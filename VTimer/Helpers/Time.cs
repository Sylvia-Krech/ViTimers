using System;
using VTimer.Consts;

namespace VTimer.Helpers;


public static class EorzeanTime {
    public static readonly int WeatherWindowDuration = 8;
    public static readonly int SecondsInEorzeanHour = 175;
    public static readonly int SecondsInWeatherWindow = WeatherWindowDuration * SecondsInEorzeanHour;

    public static long now(){
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    } 

    public static uint unixToWeatherNumber(long unix) {
        // Get Eorzea hour for weather start (every ingame hour is 175 seconds)
        int bell = (int)(unix / 175);
        // Do the magic 'cause for calculations 16:00 is 0, 00:00 is 8 and 08:00 is 16
        int increment = (bell + 8 - (bell % 8)) % 24;
        // Take Eorzea days since unix epoch
        uint totalDays = (uint)(unix / 4200) >>> 0;
        uint seed = totalDays * 100 + (uint)increment;
        uint step1 = (seed << 11) ^ seed;
        uint step2 = (step1 >>> 8) ^ step1;
        return (uint)step2;
        //return (int)(step2 % 100);
    }

    public static int unixToWeatherNumberModZoneRange(long unix, int zoneRange) {
        return (int)(unixToWeatherNumber(unix) % zoneRange);
    }

    //TODO: Optimize these loops, as each entry is a list alternating between weathers and ranges, dont need to check all of them, just the right ones.
    public static Consts.Weathers weatherFromUnix(Zones zone, long unix) {
        var weatherArray = WeatherList.ByZone[zone];
        int zoneRange = (int)weatherArray[weatherArray.Count - 1];
        //Service.PluginLog.Verbose("Zone Range " + zoneRange);
        int weatherNumber = EorzeanTime.unixToWeatherNumberModZoneRange(unix, zoneRange) ;
        //Service.PluginLog.Verbose("Weather Number " + weatherNumber);
        Consts.Weathers zoneWeather = Consts.Weathers.NA;
        foreach(var w in weatherArray) {
            if (w is Weathers){
                //this is so much of a hack and i hate it, the compiler should know the only w where is if w is a weather -_-
                zoneWeather = (Weathers)w;
            } else if ((int)w > weatherNumber) {
                break;
            }
        }
        return zoneWeather;
    }

    public static Consts.Weathers getCurrentWeather(Zones zone){
        return EorzeanTime.weatherFromUnix(zone, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    }

    public static int getEorzeanHour(long unix) {
        int bell = (int)(unix / 175);
        // Do the magic 'cause for calculations 16:00 is 0, 00:00 is 8 and 08:00 is 16
        int hour = bell % 24;
        return hour;
    }
    public static int getEorzeanMinute(long unix) {
        int secondsIntoThisEorzeanHour = (int)(unix % 175);
        double portionOfHour = (double)secondsIntoThisEorzeanHour / 175.0;
        int minutes = (int)(60.0 * portionOfHour); 
        return minutes;
    }

    public static string getEorzeanTime(long unix) {
        int hour = EorzeanTime.getEorzeanHour(unix);
        int minute = EorzeanTime.getEorzeanMinute(unix);
        return hour.ToString() + ":" + (minute < 10 ? "0" : "") + minute.ToString();
    }
    public static string getCurrentEorzeanTime() {
        return getEorzeanTime(now());
    }

    public static long getCurrentWeatherNumber() {
        long now = EorzeanTime.now();
        return EorzeanTime.unixToWeatherNumber(now);
    }
    public static long getCurrentWeatherNumberModZoneRange(int zoneRange) {
        long now = EorzeanTime.now();
        return EorzeanTime.unixToWeatherNumberModZoneRange(now, zoneRange);
    }

    internal static string delayToTimeText(long delay) {
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

    // returns the unix timestamp when targetHour and targetMinute appear after "daysToSkip" days, starting from start.
    // for normal usage pass EorzeanTime.now() as start, which will make it search from the present.
    // daysToSkip can also be negative, for use incase of looking for example, the 12:00 *before* a weather window.
    public static long GetEorzeanAsUnix(long targetHour, long targetMinute, long daysToSkip, long start) {
        //guard
        if (targetHour >= 24 || targetHour < 0) {
            Service.PluginLog.Warning("Attempted to find an invalid hour of " + targetHour);
            return -1;
        }
        if (targetMinute >= 60 || targetMinute < 0) {
            Service.PluginLog.Warning("Attempted to find an invalid hour of " + targetMinute);
            return -1;
        }

        long hour = 175;
        long minute = 3;
        long now = start;
        long output = now;
        output += daysToSkip * hour * 24;
        long currentHour = EorzeanTime.getEorzeanHour(now);
        long currentMinute = EorzeanTime.getEorzeanMinute(now);
        
        while (currentHour != targetHour){
            currentHour += 1;
            currentHour %= 24;
            output += hour;
        }
        
        //due to Eorzean Time being 2 and 11/12th seconds per minute, use a rolling counter to try to keep it more in line.
        int counter = 0;
        while (currentMinute != targetMinute) {
            currentMinute += 1;
            currentMinute %= 60;
            output += minute;
            counter += 1;
            if (counter == 12){
                counter = 0;
                output -=1;
            }
        }
        return output;
    }
}

