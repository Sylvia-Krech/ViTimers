using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using VTimer.Helpers;

namespace VTimer.Windows;

public class ConfigWindow : Window, IDisposable
{

    public ConfigWindow() : base(
        "VTimer Configuration, Version: " + Service.Version) //ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(200, 200),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

    }

    public void Dispose()
    {
    }

    public override void Draw()
    {
        //ImGui.Spacing();
        if (ImGui.BeginTabBar("ConfigBar"))
        {
            if (ImGui.BeginTabItem("Main")) {
                ImGui.Text($"It is {EorzeanTime.getCurrentEorzeanTime()}");
                CurrentTimers.Draw();
                ImGui.EndTabItem();
            }
            if (ImGui.BeginTabItem("Eureka")) {
                Eureka.Draw();
                ImGui.EndTabItem();
            }            
            if (ImGui.BeginTabItem("Farms")) {
                Farms.Draw();
                ImGui.EndTabItem();
            }
            
            ImGui.EndTabBar();
        }

        ImGui.Spacing();

        ImGui.Unindent(55);
    }
}
