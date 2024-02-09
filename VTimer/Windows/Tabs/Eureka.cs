using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using VTimer.Consts;
using VTimer.Helpers;

namespace VTimer.Windows;

public class Eureka {
    public static void Draw() {
        ImGui.Text($"Alert me X seconds before a NM window:");
        var preWarn = Service.Configuration.EurekaForewarning.Value;
        ImGui.InputInt("", ref preWarn, 30, 60); 
        if (preWarn != Service.Configuration.EurekaForewarning.Value){
            Service.Configuration.EurekaForewarning.Value = preWarn;
            Service.Configuration.Save();
        }


        foreach (string name in Groups.EurekaNMs ){
            DrawTools.DrawCheckBox(name, Numbers.ZeroVal, Service.Configuration.EurekaForewarning);
        }
    }
}
