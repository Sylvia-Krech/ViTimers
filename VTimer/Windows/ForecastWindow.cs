using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using VTimer.Helpers;

namespace VTimer.Windows;

public class ForecastWindow : Window, IDisposable
{

    public ForecastWindow() : base(
        "VTimer") //ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
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
                Forecast.Draw();
                ImGui.EndTabItem();
            }
            if (ImGui.BeginTabItem("Weather")) {
                Weather.Draw();
                ImGui.EndTabItem();
            }
            
            ImGui.EndTabBar();
        }

        ImGui.Spacing();

        ImGui.Unindent(55);
    }
}
