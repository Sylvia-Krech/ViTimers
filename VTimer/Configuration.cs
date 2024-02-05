using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace VTimer
{
    [Serializable]
    public class PluginConfiguration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;

        // Eureka Tab
        public int EurekaPreWarn = 180;
        public bool Pazuzu { get; set; } = false; //gales + night
        public bool Crab { get; set; } = false; //fog
        public bool Cassie { get; set; } = false; //blizzards
        public bool Luigi { get; set; } = false; //j night?
        public bool Skoll { get; set; } = false; //blizzards
        public bool Penny { get; set; } = false; //heat waves
        public bool ColdBox { get; set; } = false;
        public bool HeatBox { get; set; } = false;
        //public bool MoistBox { get; set; } = false;

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
