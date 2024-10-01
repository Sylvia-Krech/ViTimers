using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using VTimer.Consts;
using VTimer.Helpers;

namespace VTimer.Windows;

public class CustomTab {
    public static void Draw() {
        ImGui.Text($"A tool for creating your own weather/time based notifications");
        ImGui.Text($"Name");
        ImGui.Text($"Zone");
        ImGui.Text($"Weather");
        ImGui.Text($"# of chain occurences");
        ImGui.Text($"Time");
    }
}
