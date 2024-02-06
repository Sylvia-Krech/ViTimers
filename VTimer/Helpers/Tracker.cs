using System.Collections.Generic;
using VTimer.Consts;


namespace VTimer.Helpers;

public class Tracker {
    internal string name;
    internal Conditions condition;
    internal int forewarning;
    internal long previousWindow;
    internal List<long> nextWindows = new(); // TRUE time for windows, do not add forewarning.

    public Tracker(string n, Conditions c, ref int fw) {
        this.name = n;
        this.condition = c;
        this.forewarning = fw;
        this.findAnotherWindow();
        while (this.getNextWindow() < Service.ETM.now()) {
            this.recycle();
        }
        Service.PluginLog.Verbose(name + " Finalized, it is up in " + (this.getNextWindow() - Service.ETM.now()).ToString() + " seconds.");
    }

    public Tracker(string n, Consts.Zones z, Consts.Weathers w, Consts.dayCycle dc, int rw, ref int fw)
        : this(n, new Conditions(z, new List<Weathers>{w}, dc, rw), ref fw){}

    public long getNextWindow(){
        return nextWindows[0];
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
        this.previousWindow = this.nextWindows[0];
        this.nextWindows.RemoveAt(0);
    }
}
