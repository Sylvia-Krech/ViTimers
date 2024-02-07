using System.Collections.Generic;
using System.Xml;
using ImGuiNET;
using VTimer.Consts;


namespace VTimer.Helpers;

public class Tracker {
    internal string name;
    internal Conditions condition;
    internal KeyVal<string, int> forewarning;
    internal long previousWindowStart;
    internal long previousWindowEnd;
    internal List<long> nextWindows = new(); // TRUE time for windows, do not add forewarning.

    public Tracker(string n, Conditions c, ref KeyVal<string, int> fw) {
        this.name = n;
        this.condition = c;
        this.forewarning = fw;
        this.findAnotherWindow();
        while (this.getNextWindowInQueue() < Service.ETM.now()) {
            this.recycle();
        }
        Service.PluginLog.Verbose(name + " Finalized, it is up in " + (this.getNextWindowInQueue() - Service.ETM.now()).ToString() + " seconds.");
    }

    public Tracker(string n, Consts.Zones z, Consts.Weathers w, Consts.dayCycle dc, int rw, ref KeyVal<string, int> fw)
        : this(n, new Conditions(z, new List<Weathers>{w}, dc, rw), ref fw){}

    public long getNextWindowInQueue(){
        return nextWindows[0];
    }

    public long getUpcommingWindow(){
        if (this.previousWindowStart > Service.ETM.now()) {
            return this.previousWindowStart;
        }
        return getNextWindowInQueue();
    }

    public long lastWindow() {
        return nextWindows[nextWindows.Count -1];
    }

    public void findAnotherWindow(){
        var time = Service.ETM.findNextWindow(this);
        nextWindows.Add(time);
        if (time > Service.ETM.now()) {
            Service.PluginLog.Verbose("Created "+ name + " tracker, it is up in " + (time - Service.ETM.now()).ToString() + " seconds." +
            " At " + Service.ETM.getEorzeanTime(time) + "ET");
        }

    }

    public void recycle() {
        this.findAnotherWindow();
        this.previousWindowStart = this.nextWindows[0];
        this.previousWindowEnd = this.condition.unixOfWindowEnd(this.getNextWindowInQueue());
        this.nextWindows.RemoveAt(0);
    }

    public int getForewarning(){
        return this.forewarning.Value;
    }

    private long getGap(){
        return this.getNextWindowInQueue() - this.previousWindowStart;
    }

    public void notify() {
        string output = "[VTimer] " + this.name + " is up" + (this.getForewarning() == 0 ? "." : " in " + (this.getNextWindowInQueue() - Service.ETM.now()) + " seconds.");
        if (this.forewarning.Key == "Eureka") {
            long minutesAgo = (this.getGap() + 1) / 61;
            if (minutesAgo < 180){
                output += " It was last up " + minutesAgo + " minutes ago, it may not spawn if the oldest person in instance has <" + (180 - minutesAgo) + " mins remaining";
            } 
        }
        Service.Chat.Print(output);
    }

    public void isUpNextInText() {
        string output = this.name;
        if (this.previousWindowEnd < Service.ETM.now()){
            output += " is up next in " + Service.ETM.delayToTime(this.getUpcommingWindow() - Service.ETM.now());
            ImGui.TextColored(ConstantVars.CurrentlyDownColor, output);
        } else {
            output += " is up now, for " + Service.ETM.delayToTime(this.previousWindowEnd - Service.ETM.now());
            ImGui.TextColored(ConstantVars.CurrentlyUpColor, output);
        }
    }
}
