using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Internal.Windows.Settings.Widgets;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using VTimer.Consts;
using VTimer.Helpers;

namespace VTimer.Windows;

public class Weather {
    public static void Draw(Plugin plugin) {
        var weatherNumber = plugin.ETM.getCurrentWeatherNumber();
        ImGui.Text($"This is proof the Weather tab works");
        //ImGui.Text($"Current Timestamp: {plugin.ETM.startTime}");
        ImGui.Text($"The current weather # is: {weatherNumber}");
        foreach(var zone in WeatherList.ByZone)
        {
            //ImGui.Text($"This is proof the Weather tab works");
            var zoneName = zone.Key;
            //ImGui.Text($"This is proof the Weather zone.Key");
            var zoneWeather = plugin.ETM.getCurrentWeather(zoneName);
            if (zoneName == Consts.Zones.LimsaLominsa || zoneName == Consts.Zones.Gridania || zoneName == Consts.Zones.Uldah || zoneName == Consts.Zones.Ishgard || zoneName == Consts.Zones.RhalgrsReach){
                ImGui.Text("");
            }
            ImGui.Text($"{zoneName}: {zoneWeather}");
        }
        
        ImGui.Text($"This is proof the Weather tab works");
    }
}
