using System;
using System.Collections.Generic;
using Lumina.Excel.GeneratedSheets;
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
        Service.PluginLog.Verbose(name + "Finalized, it is up in " + (this.getNextWindow() - Service.ETM.now()).ToString() + " seconds.");
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
        Service.PluginLog.Verbose("Created "+ name + " tracker, it is up in " + (time - Service.ETM.now()).ToString() + " seconds.");
    }

    public void recycle() {
        this.findAnotherWindow();
        this.previousWindow = this.nextWindows[0];
        this.nextWindows.RemoveAt(0);
    }
}

public class Conditions {
    internal Zones zone;
    internal dayCycle dayCycle;
    internal List<Weathers> weathers;
    internal int repeatWeathers;
    public Conditions (Zones z, List<Weathers> w, dayCycle dc, int rw){
        this.zone = z;
        this.dayCycle = dc;
        this.weathers = w;
        this.repeatWeathers = rw;
    }

    internal bool HasNoWeatherCondition()
    {
        return weathers.Count == 0;
    }

    internal bool isBellWithinDayCycle(int bell)
    {
        if (this.dayCycle == dayCycle.NA) {
            return true;
        } else if (this.dayCycle == dayCycle.Day && (bell >= 6 || bell < 18)) {
            return true;
        } else if (this.dayCycle == dayCycle.Night && (bell < 6 || bell >= 18)) {
            return true;
        }
        return false;
    }

    internal bool isThisWeatherValid(Weathers w){
        return this.weathers.Contains(w);
    }
}