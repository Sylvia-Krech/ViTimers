using System.Collections.Generic;
using VTimer.Consts;
using ImGuiNET;


namespace VTimer.Helpers;
public class Condition {
    static long MAX_CHAIN_TO_CHECK = 50;
    static long MAX_WINDOWS_TO_CHECK = 1000;

    internal long unixIncrements; //time for a full cycle
    internal long offset;
    internal long unixDuration; //condition always starts at 0s per Increment, and lasts unixDuration time
    public (long, long) findNextWindow(long startSearch) {
        return this.findNextWindow(startSearch, 0);
    }

    public (long, long) findNextWindow(long startSearch, long depth){
        if (depth > MAX_CHAIN_TO_CHECK) {
            Service.PluginLog.Error("Somehow went " + MAX_CHAIN_TO_CHECK + " chains deep without finding an end?");
            return (-1,-1);
        };
        startSearch -= 1; //hack?
        long start = startSearch + (unixIncrements - (startSearch % unixIncrements)) + offset;
        long end = start + unixDuration;
        //Service.PluginLog.Verbose("startSearch % unixIncrements: " + (startSearch % unixIncrements));

        long counter = 0;
        while ( !windowValid(start) ) {
            counter++;
            if (counter > MAX_WINDOWS_TO_CHECK) { Service.PluginLog.Error("Somehow went " + MAX_WINDOWS_TO_CHECK + " windows deep without a window?"); break;}
            start = start + unixIncrements;
            end = start + unixDuration;
        } 

        // Handle chain windows, this shit sucks.
        /*
        if (unixIncrements == unixDuration) {
            (long newStart, long newEnd) = findNextWindow(end+1, depth + 1);
            if (newStart == end) {
                end = newEnd;
            }
        } */
        return (start, end);
    }

    //If no window check logic is provided, assume every cycle perfectly describes the conditions uptime 
    // For things like Realtime constant conditions, such as dayCycle, and RealTime conditions, obviously.
    internal virtual bool windowValid(long start) {
        return true;
    }

    public long unixOfWindowEnd (long start) {
        return this.findNextWindow(start).Item2;
    }
}

public class Weather_Condition : Condition {
    internal Zones zone;
    internal List<Weathers> weathers;
    internal List<Weathers> requiredPreviousWeather = new();
    public Weather_Condition (Zones z, List<Weathers> w){
        this.zone = z;
        this.weathers = w;
        this.unixIncrements = 175 * 8;
        this.offset = 0;
        this.unixDuration = 175 * 8;
    }

    public Weather_Condition (Zones z, List<Weathers> w, List<Weathers> pw){
        this.zone = z;
        this.weathers = w;
        this.requiredPreviousWeather = pw;
    }

    internal bool isThisWeatherValid(Weathers w){
        return this.weathers.Contains(w);
    }

    //doesnt check for repeat weathers
    internal override bool windowValid(long unix) {
        if (!this.isThisWeatherValid(EorzeanTime.weatherFromUnix(this.zone, unix))) { 
            return false; 
        }
        return true;
    }
}

public class RealTimeConditions : Condition {
    internal long fullCycle;
    internal long startsAt;
    internal long duration;
    internal long forewarning;
    public RealTimeConditions (long _fullCycle, long _offset, long _duration) {
        this.unixIncrements = _fullCycle;
        this.offset = _offset;
        this.unixDuration = _duration;
    }

}
public class DayCycleCondition : Condition {
    public DayCycleCondition (dayCycle dc){
        this.unixDuration = 175 * 12;
        this.offset = 175 * 6 + ( dc == dayCycle.Day ? 0 : 175 * 12);
        this.unixIncrements = 175 * 24;
    }
}