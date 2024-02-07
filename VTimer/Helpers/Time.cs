using System;
using Lumina.Excel.GeneratedSheets;
using VTimer.Consts;

namespace VTimer.Helpers;
public class EorzeanTimeManager {

    //constructor
    public EorzeanTimeManager(){
    }

    public long now(){
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    } 
    //functions
    public int unixToWeatherNumber(long unix) {
        // Get Eorzea hour for weather start (every ingame hour is 175 seconds)
        int bell = (int)(unix / 175);
        // Do the magic 'cause for calculations 16:00 is 0, 00:00 is 8 and 08:00 is 16
        int increment = (bell + 8 - (bell % 8)) % 24;
        // Take Eorzea days since unix epoch
        int totalDays = (int)(unix / 4200) >>> 0;
        int calcBase = totalDays * 100 + increment;
        int step1 = ((calcBase << 0xB) ^ calcBase) >>> 0;
        int step2 = ((step1 >>> 8) ^ step1) >>> 0;
        return step2 % 100;
    }

    public Consts.Weathers weatherFromUnix(Zones zone, long unix) {
        var weatherArray = WeatherList.ByZone[zone];
        int weatherNumber = this.unixToWeatherNumber(unix) ;
        Consts.Weathers zoneWeather = Consts.Weathers.NA;
        foreach(var w in WeatherList.ByZone[zone]) {
            if (w is Weathers){
                //this is so much of a hack and i hate it, the compiler should know the only here is if w is a weather -_-
                zoneWeather = (Weathers)w;
            } else if ((int)w > weatherNumber) {
                break;
            }
        }
        return zoneWeather;
    }

    public Consts.Weathers getCurrentWeather(Zones zone){
        return this.weatherFromUnix(zone, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }

    public int getEorzeanHour(long unix) {
        int bell = (int)(unix / 175);
        // Do the magic 'cause for calculations 16:00 is 0, 00:00 is 8 and 08:00 is 16
        int hour = bell % 24;
        return hour;
    }
    public int getEorzeanMinute(long unix) {
        int secondsIntoThisEorzeanHour = (int)(unix % 175);
        double portionOfHour = (double)secondsIntoThisEorzeanHour / 175.0;
        int minutes = (int)(60.0 * portionOfHour); 
        return minutes;
    }

    public string getEorzeanTime(long unix) {
        int hour = this.getEorzeanHour(unix);
        int minute = this.getEorzeanMinute(unix);
        return hour.ToString() + ":" + (minute < 10 ? "0" : "") + minute.ToString();
    }
    public string getCurrentEorzeanTime() {
        return getEorzeanTime(now());
    }

    public long getCurrentWeatherNumber() {
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        return this.unixToWeatherNumber(now);
    }

    //TODO make this a part of Conditions, makes more sense there anyways.
    //I am midly worried this will become a mess
    //TODO make it more intelligently check based off of parameters, i.e. if only weather, do increments of 8 hours on the reset line
    public long findNextWindow(long start, Conditions condition) {
        long now = start;
        now += 175 - (now % 175); //round up to next nearest hour 
        int numRepeated = 0;
        long repeatWeathersStart = 0;
        //skip current weather/daycycle if it is the target weather
        now = condition.unixOfWindowEnd(now);
        int bell = this.getEorzeanHour(now);
        
        int failsafe = 0;
        while (true){
            now += 175;
            bell += 1;
            bell = bell%24;
            //Service.PluginLog.Verbose("Weather in " + condition.zone.ToString() + " at " + now.ToString() + " is " + this.weatherFromUnix(condition.zone, now));
            failsafe += 1;
            if (failsafe >= 10000) {
                Service.PluginLog.Warning("Next window failed to be found within " + failsafe.ToString() + " iterations");
                return 0 ;
            }
            
            // check day
            if (!condition.isBellWithinDayCycle(bell)){
                continue;
            }

            //check weather
            if (condition.HasNoWeatherCondition() || condition.isThisWeatherValid(this.weatherFromUnix(condition.zone, now))) {
                if (condition.repeatWeathers == 0) {
                    break;
                } else {
                    if (numRepeated == 0) {
                        repeatWeathersStart = now;
                    }
                    numRepeated += 1;
                    if (numRepeated == condition.repeatWeathers) {
                        break;
                    }
                    continue; //to bypass the reset below
                }
            }
            numRepeated = 0; //maybe move into an else?
        }

        if (condition.repeatWeathers != 0) {
            return repeatWeathersStart;
        }
        return now;
    }

    internal long findNextWindow(Tracker tracker)
    {
        //Service.PluginLog.Verbose("Queue Length: " + tracker.nextWindows.Count.ToString());
        if (tracker.nextWindows.Count == 0) {
            return this.findNextWindow(this.now() - (180 * 60), tracker.condition);
        }
        return this.findNextWindow(tracker.lastWindow(), tracker.condition);
    }

    internal string delayToTime(long delay) {
        long seconds = delay % 60;
        delay /= 60;
        string sec = seconds.ToString();
        if (sec.Length == 1) {
            sec = "0" + sec;
        }

        long minutes = delay % 60;
        delay /= 60;
        string min = minutes.ToString();
        if (min.Length == 1) {
            min = "0" + min;
        }
        
        long hours = delay % 24;
        delay /= 24;
        string hrs = hours.ToString();
        if (hrs.Length == 1) {
            hrs = "0" + hrs;
        }
        
        long days = delay;
        return days.ToString() + ":" + hrs + ":" + min + ":" + sec;
    }
}

