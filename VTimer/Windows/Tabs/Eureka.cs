using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using VTimer.Consts;
using VTimer.Helpers;

namespace VTimer.Windows;

public class Eureka {
    public static void Draw(Plugin plugin) {
        // I hate that I cannot turn this mess into a pre-processor macro, if there is a better way than massive code duplication
        // please let me know

        ImGui.Text($"Alert me X seconds before a NM window:");
        var preWarn = Service.Configuration.EurekaPreWarn;
        ImGui.InputInt("", ref preWarn, 30, 60); 
        if (preWarn != Service.Configuration.EurekaPreWarn){
            Service.Configuration.EurekaPreWarn = preWarn;
            Service.Configuration.Save();
        }
        var Pazuzu = Service.Configuration.Pazuzu;
        if(ImGui.Checkbox("Pazuzu", ref Pazuzu)){
            Service.PluginLog.Verbose("Charlie");
            Service.Configuration.Pazuzu = Pazuzu;
            PresetTimers.AddOrRemoveTimer("Pazuzu", ref Service.Configuration.EurekaPreWarn);
            Service.Configuration.Save();
        }
        var Crab = Service.Configuration.Crab;
        if(ImGui.Checkbox("Crab", ref Crab)){
            Service.Configuration.Crab = Crab;
            PresetTimers.AddOrRemoveTimer("Crab", ref Service.Configuration.EurekaPreWarn);
            Service.Configuration.Save();
        }
        var Cassie = Service.Configuration.Cassie;
        if(ImGui.Checkbox("Cassie", ref Cassie)){
            Service.Configuration.Cassie = Cassie;
            PresetTimers.AddOrRemoveTimer("Cassie", ref Service.Configuration.EurekaPreWarn);
            Service.Configuration.Save();
        }
        var Luigi = Service.Configuration.Luigi;
        if(ImGui.Checkbox("Luigi", ref Luigi)){
            Service.Configuration.Luigi = Luigi;
            PresetTimers.AddOrRemoveTimer("Luigi", ref Service.Configuration.EurekaPreWarn);
            Service.Configuration.Save();
        }
        var Skoll = Service.Configuration.Skoll;
        if(ImGui.Checkbox("Skoll", ref Skoll)){
            Service.Configuration.Skoll = Skoll;
            PresetTimers.AddOrRemoveTimer("Skoll", ref Service.Configuration.EurekaPreWarn);
            Service.Configuration.Save();
        }
        var Penny = Service.Configuration.Penny;
        if(ImGui.Checkbox("Penny", ref Penny)){
            Service.Configuration.Penny = Penny;
            PresetTimers.AddOrRemoveTimer("Penny", ref Service.Configuration.EurekaPreWarn);
            Service.Configuration.Save();
        }
        var ColdBox = Service.Configuration.ColdBox;
        if(ImGui.Checkbox("ColdBox", ref ColdBox)){
            Service.Configuration.ColdBox = ColdBox;
            Service.Configuration.Save();
        }
        var HeatBox = Service.Configuration.HeatBox;
        if(ImGui.Checkbox("HeatBox", ref HeatBox)){
            Service.Configuration.HeatBox = HeatBox;
            Service.Configuration.Save();
        }/* 
        var MoistBox = Service.Configuration.MoistBox;
        if(ImGui.Checkbox("MoistBox", ref MoistBox)){
            Service.Configuration.MoistBox = MoistBox;
            Service.Configuration.Save();
        } */
    }
}
