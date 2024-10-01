using System;
using Lumina.Excel.GeneratedSheets;
using VTimer.Consts;

namespace VTimer.Helpers;


public static class EorzeanTime {
    public static readonly int WeatherWindowDuration = 8;
    public static readonly int SecondsInEorzeanHour = 175;
    public static readonly int SecondsInWeatherWindow = WeatherWindowDuration * SecondsInEorzeanHour;

    public static long now(){
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    } 

    public static int unixToWeatherNumber(long unix) {
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

    public static Consts.Weathers weatherFromUnix(Zones zone, long unix) {
        var weatherArray = WeatherList.ByZone[zone];
        int weatherNumber = EorzeanTime.unixToWeatherNumber(unix) ;
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
}

