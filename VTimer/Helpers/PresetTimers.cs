using System.Collections;
using System.Collections.Generic;
using VTimer.Consts;

namespace VTimer.Helpers;

class PresetTimers {
    public static void LoadTimers() {
        Service.PluginLog.Verbose("Loading Preset Timers");
        foreach (KeyValuePair<string,bool> state in Service.Configuration.TrackerState) {
            if (state.Value) {
                Service.PluginLog.Verbose("Attempting to add " + state.Key);
                PresetTimers.AddTimer(state.Key, Numbers.ZeroVal, Service.Configuration.EurekaForewarning);
            }
        }
    }

    public static bool trackerExists(string name) {
        for (int i = 0; i < Service.Trackers.Count; ++i) {
            if (Service.Trackers[i].name == name) {
                return true;
            }
        }
        return false;
    }

    public static void AddOrRemoveTimer(string name, Val<int> minimumDuration, Val<int> forewarning){
        if (!trackerExists(name)) {
            AddTimer(name, minimumDuration, forewarning);
        } else {
            removeTracker(name);
        }
    }
    public static void AddTimer(string name, Val<int> minDuration, Val<int> forewarning){
        if (!trackerExists(name)) {
            var condition = Presets.Conditions[name];
            Service.Trackers.Add(new Tracker(name, condition, minDuration, forewarning));
        } else {
            Service.PluginLog.Warning("Attempted to call AddTimer when a timer of the same name already exists");
        }
    }


    public static void removeTracker(string name) {
        if (trackerExists(name)) {
            for (int i = 0; i < Service.Trackers.Count; ++i) {
                if (Service.Trackers[i].name == name) {
                    Service.Trackers.RemoveAt(i);
                    Service.PluginLog.Verbose("Removed Tracker:" + name);
                    return;
                }
            }
        }
    }
}
