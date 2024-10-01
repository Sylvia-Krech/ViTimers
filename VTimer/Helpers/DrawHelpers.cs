using System;
using ImGuiNET;
using VTimer.Helpers;
namespace VTimer.Windows;

class DrawTools {
    public static void DrawCheckBox(string name, Val<int> duration, in Val<int> forewarning) {
        bool state = Service.Configuration.TrackerState[name];
        if(ImGui.Checkbox(name, ref state)){
            Service.Configuration.TrackerState[name] = state;
            PresetTimers.AddOrRemoveTimer(name, duration, forewarning);
            Service.Configuration.Save();
        }
    }


    public static void DrawWindowPair(Timestamp timestamp) {
        string localTime = DateTimeOffset.FromUnixTimeSeconds(timestamp.start).ToLocalTime().ToString();
        //trim the timezone and seconds
        localTime = localTime.Substring(0, localTime.Length-13) + localTime.Substring(localTime.Length - 10, 3);
        ImGui.TextColored(timestamp.statusColor(),localTime);
        ImGui.TableNextColumn();
        ImGui.Text(timestamp.tracker.name);
    }
}
