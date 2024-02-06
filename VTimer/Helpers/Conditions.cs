using System.Collections.Generic;
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
        return isTimeValid(unix, Service.ETM.getEorzeanHour(unix));
    }

    //doesnt check for repeat weathers
    internal bool isTimeValid(long unix, int bell) {
        if (!this.isBellWithinDayCycle(bell)){ return false; }
        if (!this.HasNoWeatherCondition()) {
            if (!this.isThisWeatherValid(Service.ETM.weatherFromUnix(this.zone, unix))) { return false; }
        }
        return true;
    }
}