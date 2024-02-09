using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Xml;
using ImGuiNET;
using VTimer.Consts;


namespace VTimer.Helpers;

public class Tracker {
    internal static long backSearch = 180 * 60;
    internal string name;
    internal Conditions condition;
    internal long previousWindowStart;
    internal readonly Val<int> forewarning;
    internal readonly Val<int> minDuration; 
    internal long previousWindowEnd;
    private List<long> nextWindows = new(); // TRUE time for windows, do not add forewarning.

    public Tracker(string n, Conditions c, Val<int> minDur, Val<int> fw) {
        this.name = n;
        this.condition = c;
        this.forewarning = fw;
        this.minDuration = minDur;
        this.findAnotherWindow();
        while (this.firstWindow() < EorzeanTime.now()) {
            this.recycle();
        }
        Service.PluginLog.Verbose(name + " Finalized, it is up in " + (this.firstWindow() - EorzeanTime.now()).ToString() + " seconds.");
    }

    public Tracker(string n, Consts.Zones z, Consts.Weathers w, Consts.dayCycle dc, ref Val<int> dur, ref Val<int> fw)
        : this(n, new Conditions(z, new List<Weathers>{w}, dc), dur, fw){}


    public bool hasWindowInQueue(){
        return nextWindows.Count != 0;
    }
    public long firstWindow(){
        return nextWindows.First();
    }
    public long lastWindow() {
        return nextWindows.Last();
    }

    public long getUpcommingWindow(){
        if (this.previousWindowStart > EorzeanTime.now()) {
            return this.previousWindowStart;
        }
        return firstWindow();
    }


    public void findAnotherWindow(){
        long time = 0;
        if (this.hasWindowInQueue()){
            time = condition.findNextWindow(this.lastWindow());
        } else {
            time = condition.findNextWindow(EorzeanTime.now() - backSearch);
        }

        if (time == 0) { 
            Service.PluginLog.Warning("Window for " + this.name + " failed to be found, are you sure that the settings for it are correct?");
            return;
        } 

        nextWindows.Add(time);
        if (time > EorzeanTime.now()) {
            Service.PluginLog.Verbose("Created "+ name + " tracker, it is up in " + (time - EorzeanTime.now()).ToString() + " seconds." +
            " At " + EorzeanTime.getEorzeanTime(time) + "ET");
        }
    }

    public void recycle() {
        this.findAnotherWindow();
        this.previousWindowStart = this.nextWindows[0];
        this.previousWindowEnd = this.condition.unixOfWindowEnd(this.firstWindow());
        this.nextWindows.RemoveAt(0);
    }

    public int getForewarning(){
        return this.forewarning.Value;
    }

    private long getGap(){
        return this.firstWindow() - this.previousWindowEnd;
    }

    public void notify() {
        string output = "[VTimer] " + this.name + " is up" + (this.getForewarning() == 0 ? "." : " in " + (this.firstWindow() - EorzeanTime.now()) + " seconds.");
        if (Groups.EurekaNMs.Contains(this.name)) {
            long minutesAgo = (this.getGap() + 1) / 61;
            if (minutesAgo < 180){
                output += " It was last up " + minutesAgo + " minutes ago, it may not spawn if the oldest person in instance has <" + (180 - minutesAgo) + " mins remaining";
            } 
        }
        Service.Chat.Print(output);
    }

    public void isUpNextInText() {
        //Service.PluginLog.Verbose("Attempting to draw " + this.name + "'s timer to the screen");
        string output = this.name;
        if (this.previousWindowEnd < EorzeanTime.now()){
            output += " is up next in " + EorzeanTime.delayToTimeText(this.getUpcommingWindow() - EorzeanTime.now());
            ImGui.TextColored(Colors.CurrentlyDown, output);
        } else if (this.previousWindowStart > EorzeanTime.now()) {
            output += " is up soon, in " + EorzeanTime.delayToTimeText(this.getUpcommingWindow() - EorzeanTime.now());
            ImGui.TextColored(Colors.UpSoon, output);
        } else {
            output += " is up now, for " + EorzeanTime.delayToTimeText(this.previousWindowEnd - EorzeanTime.now());
            ImGui.TextColored(Colors.CurrentlyUp, output);
        }
    }
}
