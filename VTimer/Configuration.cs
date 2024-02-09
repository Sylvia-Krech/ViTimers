using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;
using VTimer.Consts;
using VTimer.Helpers;

namespace VTimer
{
    [Serializable]
    public class PluginConfiguration : IPluginConfiguration
    {
        // Anything that will be saved to the config file must be public, not internal.
        public int Version { get; set; } = 0;

        public Val<int> EurekaForewarning = new(180);
        public Val<int> FarmForewarning = new(180);
        // In Eorzean Hours
        public Val<int> FarmMinDuration = new(24);

        public Dictionary<string, bool> TrackerState = new Dictionary<string, bool> {
            {Names.Pazuzu, false},
            {Names.Crab, false},
            {Names.Cassie, false},
            {Names.Luigi, false},
            {Names.Skoll, false},
            {Names.Penny, false},

            {Names.ColdBox, false},
            {Names.HeatBox, false},
            {Names.Preparation, false},
            {Names.Care, false},
            {Names.Support, false},
            {Names.History, false},
            {Names.Artistry, false}
        };


        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private DalamudPluginInterface? PluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.PluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.PluginInterface!.SavePluginConfig(this);
        }
    }
}
