using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Xml;
using ImGuiNET;
using VTimer.Consts;


namespace VTimer.Helpers;

public class Tracker {
    internal static long backSearch = 180 * 60;
    internal static long MAX_WINDOWS_TO_CHECK = 10000;
    internal string name;
    internal bool notified = false;
    internal List<Condition> conditions;
    internal Timestamp previousWindow;
    internal readonly Val<int> forewarning;
    internal readonly Val<int> minDuration; 
    internal List<Timestamp> nextWindows = new(); // TRUE time for windows, do not add forewarning.
    
    public Tracker(string n, List<Condition> c, Val<int> minDur, Val<int> fw) {
        this.name = n;
        this.conditions = c;
        this.forewarning = fw;
        this.minDuration = minDur;
        this.findAnotherWindow();
        this.previousWindow = new(0, 0, this);
        while (this.endOfFirstWindow() < EorzeanTime.now()) {
            this.recycle();
        }
        Service.PluginLog.Verbose(name + " Finalized, it is up in " + (this.startOfFirstWindow() - EorzeanTime.now()).ToString() + " seconds.");
    }


    public Tracker(string n, Consts.Zones z, Consts.Weathers w, Consts.dayCycle dc, ref Val<int> dur, ref Val<int> fw)
        : this(n, new Weather_Condition(z, new List<Weathers>{w}), dur, fw){}

    public Tracker(string n, Weather_Condition c, Val<int> minDur, Val<int> fw) 
        : this(n, new List<Condition>{c}, minDur, fw){}

    public bool hasWindowInQueue(){
        return nextWindows.Count != 0;
    }

    public long startOfFirstWindow() {
        return nextWindows.First().start;
    }
    
    public long endOfFirstWindow() {
        return nextWindows.First().end;
    }

    public long startOfLastWindow() {
        return nextWindows.First().start;
    }

    public long endOfLastWindow() {
        return nextWindows.Last().end;
    }

    public long numberOfWindowsInQueue() {
        return nextWindows.Count;
    }

    public long getUpcommingWindow(){
        if (this.previousWindow.start > EorzeanTime.now()) {
            return this.previousWindow.start;
        }
        return startOfFirstWindow();
    }


    public void findAnotherWindow(){
        long startSearch = 0;
        List<(long, long)> windows = new List<(long, long)>(){};
        if (this.hasWindowInQueue()){
            long baseSearch = this.endOfLastWindow() - 1; //hack on the -1, i dont get why this fixed it, ontop of other -1 in condition's search
            foreach (Condition cond in conditions) {
                long n = cond.unixOfWindowEnd(baseSearch);
                startSearch = (startSearch < n ) ? n : startSearch;
            }
        } else {
            startSearch = EorzeanTime.now() - backSearch ;
        }

        // Add earliest window of each condition to list
        foreach (Condition cond in conditions) {
            windows.Add(cond.findNextWindow(startSearch - cond.unixIncrements));
        }

        // Cycle through windows until we find overlap. Each time we don't, find the next window of the condition that ends the soonest
        long start = 0;
        long counter = 0;
        long end = long.MaxValue;
        while (true) {
            start = 0;
            end = long.MaxValue;
            int minIndex = int.MaxValue;
            for (int i = 0; i < windows.Count; i++) {
                (long ,long) current = windows[i];
                if (current.Item1 > start) {
                    start = current.Item1;
                }
                if (current.Item2 < end) {
                    end = current.Item2;
                    minIndex = i;
                }
            }
            Service.PluginLog.Verbose("Start: " + start + "End: " + end);
            
            if ((start < end) && (!this.hasWindowInQueue() || this.endOfLastWindow() != end)) {
                break;
            } 
            windows.RemoveAt(minIndex);
            windows.Insert(minIndex, conditions[minIndex].findNextWindow(start));
            counter++;
            if (counter > MAX_WINDOWS_TO_CHECK) {
                start = 0;
                end = 0;
                break;
            }
        }

        (long, long) time = (start, end);
        if (time.Item1 == 0) { 
            Service.PluginLog.Warning("Window for " + this.name + " failed to be found, are you sure that the settings for it are correct?");
            return;
        } 

        Timestamp ts = new Timestamp(time, this);
        nextWindows.Add(ts);

        if (time.Item1 > EorzeanTime.now()) {
            Service.PluginLog.Verbose("Created "+ name + " tracker, it is up in " + (time.Item1 - EorzeanTime.now()).ToString() + " seconds." +
            " At " + EorzeanTime.getEorzeanTime(time.Item1) + "ET");
        }
    }


    // Remove first window, and add another to the queue to replace it.
    public void recycle() {
        this.findAnotherWindow();
        this.previousWindow = this.nextWindows[0];
        this.nextWindows.RemoveAt(0);
        notified = false;
    }

    public TimestampStatus upcommingWindowStatus(){
        return this.nextWindows[0].getStatus();
    }

    public int getForewarning(){
        return this.forewarning.Value;
    }

    private long getGap(){
        return this.startOfFirstWindow() - this.previousWindow.start;
    }

    // TODO: convert what is checked here, i.e., nm/farm window to instead 2 diff functions that can be passed in as arguments and saved as a param of the tracker.
    // This should also handle "chain" windows for farms
    public void notify() {
        if (notified) { return; }
        notified = true;
        long delay = System.Math.Max(this.startOfFirstWindow() - EorzeanTime.now(), 0);
        string output = "[VTimer] " + this.name + " is up" + (delay == 0 ? "." : " in " + delay + " seconds.");
        if (Groups.EurekaNMs.Contains(this.name)) {
            long minutesAgo = this.getGap()/60;
            if (minutesAgo > 180-20 && minutesAgo < 180) {
                output += " It was last up " + minutesAgo + " mins ago, it will spawn, but it may be delayed up to " + (180 - minutesAgo) + " mins";
            } else if (minutesAgo < 180) {
                output += " It was last up " + minutesAgo + " mins ago, it may not spawn if the oldest person in instance has <" + (180 - minutesAgo) + " mins remaining";
            }   
        }
        /* I dont remember what the fuck this was for
        if (this.minDuration.Value > EorzeanTime.WeatherWindowDuration){
            if( this.conditions.unixOfWindowEnd(this.startOfFirstWindow()) < EorzeanTime.SecondsInWeatherWindow * this.minDuration.Value ){
                return;
            }
        }*/
        Service.Chat.Print(output);
    }

    public void isUpNextInText() {
        //Service.PluginLog.Verbose("Attempting to draw " + this.name + "'s timer to the screen");
        string output = this.name;
        switch (this.upcommingWindowStatus()) {
            case TimestampStatus.up:
                output += " is up now, for " + EorzeanTime.delayToTimeText(this.endOfFirstWindow() - EorzeanTime.now());
                ImGui.TextColored(Colors.up, output);
                break;

            case TimestampStatus.upSoon:
                output += " is up soon, in " + EorzeanTime.delayToTimeText(this.getUpcommingWindow() - EorzeanTime.now());
                ImGui.TextColored(Colors.upSoon, output);
                break;

            case TimestampStatus.upEventually:
                output += " is up next in " + EorzeanTime.delayToTimeText(this.getUpcommingWindow() - EorzeanTime.now());
                ImGui.TextColored(Colors.upEventually, output);
                break;
        }
    }
}