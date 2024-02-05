using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace VTimer.Windows;

enum Tab {
    Main,
    Eureka,
    Weather,
}

public class MainWindow : Window, IDisposable
{
    private Plugin Plugin;

    public MainWindow(Plugin plugin) : base(
        "VTimer Configuration") //ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.Plugin = plugin;
    }

    public void Dispose()
    {
    }

    public override void Draw()
    {
        //ImGui.Spacing();
        if (ImGui.BeginTabBar("MyTabBar"))
        {
            if (ImGui.BeginTabItem("Main")) {
                ImGui.Text($"It is {this.Plugin.ETM.getCurrentEorzeanTime()}");
                ImGui.EndTabItem();
            }
            if (ImGui.BeginTabItem("Eureka")) {
                Eureka.Draw(this.Plugin);
                ImGui.EndTabItem();
            }
            if (ImGui.BeginTabItem("Weather")) {
                Weather.Draw(this.Plugin);
                ImGui.EndTabItem();
            }
            ImGui.EndTabBar();
        }

        ImGui.Spacing();

        ImGui.Unindent(55);
    }
}
