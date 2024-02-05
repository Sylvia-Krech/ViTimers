using System.Collections;
using System.Collections.Generic;
using VTimer.Helpers;

namespace VTimer.Consts;

class PresetTimers {
    public static void LoadTimers() {
        Service.PluginLog.Verbose("Loading Preset Timers");
        //TODO add support for only garunteed spawns
        if (Service.Configuration.Pazuzu) {
            Service.PluginLog.Verbose("Attempting to add Paz");
            PresetTimers.AddTimer("Pazuzu", ref Service.Configuration.EurekaPreWarn);
        }
        if (Service.Configuration.Crab) {
            Service.PluginLog.Verbose("Attempting to add Crab");
            PresetTimers.AddTimer("Crab", ref Service.Configuration.EurekaPreWarn);
        }
        if (Service.Configuration.Cassie) {
            Service.PluginLog.Verbose("Attempting to add Cassie");
            PresetTimers.AddTimer("Cassie", ref Service.Configuration.EurekaPreWarn);
        }
        if (Service.Configuration.Luigi) {
            Service.PluginLog.Verbose("Attempting to add Luigi");
            PresetTimers.AddTimer("Luigi", ref Service.Configuration.EurekaPreWarn);
        }
        if (Service.Configuration.Skoll) {
            Service.PluginLog.Verbose("Attempting to add Skoll");
            PresetTimers.AddTimer("Skoll", ref Service.Configuration.EurekaPreWarn);
        }
        if (Service.Configuration.Penny) {
            Service.PluginLog.Verbose("Attempting to add Penny");
            PresetTimers.AddTimer("Penny", ref Service.Configuration.EurekaPreWarn);
        }
    }
    public static void AddOrRemoveTimer(string name, ref int forewarning){
        if (!doesTrackerExist(name)) {
            AddTimer(name, ref forewarning);
        } else {
            removeTracker(name);
        }
    }
    public static void AddTimer(string name, ref int forewarning){
        //{"Luigi", new ArrayList() {Zones.EurekaPagos, Weathers.NA, dayCycle.Night, 0, Service.Configuration.EurekaPreWarn} }
        if (!doesTrackerExist(name)) {
            var condition = Presets.Timers[name];
            Service.Trackers.Add(new Tracker(name, condition, ref forewarning));
        } else {
            Service.PluginLog.Warning("Attempted to call AddTimer when a timer of the same name already exists");
        }
    }

    public static bool doesTrackerExist(string name) {
        for (int i = 0; i < Service.Trackers.Count; ++i) {
            if (Service.Trackers[i].name == name) {
                return true;
            }
        }
        return false;
    }
    public static void removeTracker(string name) {
        for (int i = 0; i < Service.Trackers.Count; ++i) {
            if (Service.Trackers[i].name == name) {
                Service.Trackers.RemoveAt(i);
                Service.PluginLog.Verbose("Removed Tracker:" + name);
                return;
            }
        }
    }
}
