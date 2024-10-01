using ImGuiNET;
using VTimer.Consts;

namespace VTimer.Windows;
public class CurrentTimers {
    public static void Draw() {
        foreach(var T in Service.Trackers){
            T.isUpNextInText();
        }
        if ( ImGui.Button("Open Main VTimer Window") ) {
            Service.Plugin.ForecastWindow.IsOpen = !Service.Plugin.ForecastWindow.IsOpen;
        }
    }
}