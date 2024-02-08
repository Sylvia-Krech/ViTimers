using System.Collections.Generic;
using FFXIVClientStructs.FFXIV.Common.Component.BGCollision;
using VTimer.Consts;

namespace VTimer.Helpers;
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

    internal bool isTimeValid(long unix) {
        return isTimeValid(unix, EorzeanTime.getEorzeanHour(unix));
    }

    //doesnt check for repeat weathers
    internal bool isTimeValid(long unix, int bell) {
        if (!this.isBellWithinDayCycle(bell)){ return false; }
        if (!this.HasNoWeatherCondition()) {
            if (!this.isThisWeatherValid(EorzeanTime.weatherFromUnix(this.zone, unix))) { return false; }
        }
        return true;
    }

    internal long unixOfWindowEnd(long unix) {
        int bell = EorzeanTime.getEorzeanHour(unix);
        long failsafe = 0;
        while (this.isTimeValid(unix, bell)) {
            //Service.PluginLog.Verbose(now.ToString() + " " + bell.ToString() + " " + this.isTimeValid(now, bell).ToString());
            failsafe += 1;
            if (failsafe >= 1000) {
                Service.PluginLog.Warning("Skipping current window failed after " + failsafe.ToString() + " iterations");
                return 0 ;
            }
            
            unix += 175;
            bell += 1;
            bell = bell%24;
        }
        return unix;
    }


    //I am midly worried this will become a mess
    //TODO make it more intelligently check based off of parameters, i.e. if only weather, do increments of 8 hours on the reset line
    public long findNextWindow(long start) {
        long now = start;
        now += 175 - (now % 175); //round up to next nearest hour 
        int numRepeated = 0;
        long repeatWeathersStart = 0;
        //skip current weather/daycycle if it is the target weather
        now = this.unixOfWindowEnd(now);
        int bell = EorzeanTime.getEorzeanHour(now);
        
        int failsafe = 0;
        while (true){
            now += 175;
            bell += 1;
            bell = bell%24;
            //Service.PluginLog.Verbose("Weather in " + this.zone.ToString() + " at " + now.ToString() + " is " + EorzeanTime.weatherFromUnix(this.zone, now));
            failsafe += 1;
            if (failsafe >= 10000) {
                Service.PluginLog.Warning("Next window failed to be found within " + failsafe.ToString() + " iterations");
                return 0 ;
            }
            
            // check day
            if (!this.isBellWithinDayCycle(bell)){
                continue;
            }

            //check weather
            if (this.HasNoWeatherCondition() || this.isThisWeatherValid(EorzeanTime.weatherFromUnix(this.zone, now))) {
                if (this.repeatWeathers == 0) {
                    break;
                } else {
                    if (numRepeated == 0) {
                        repeatWeathersStart = now;
                    }
                    numRepeated += 1;
                    if (numRepeated == this.repeatWeathers) {
                        break;
                    }
                    continue; //to bypass the reset below
                }
            }
            numRepeated = 0; //maybe move into an else?
        }

        if (this.repeatWeathers != 0) {
            return repeatWeathersStart;
        }
        return now;
    }

    internal long findNextWindow(Tracker tracker)
    {
        //Service.PluginLog.Verbose("Queue Length: " + tracker.nextWindows.Count.ToString());
        if (tracker.nextWindows.Count == 0) {
            return this.findNextWindow(EorzeanTime.now() - (180 * 60));
        }
        return this.findNextWindow(tracker.lastWindow());
    }
}