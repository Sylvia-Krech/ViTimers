using ImGuiNET;
using VTimer.Consts;

namespace VTimer.Windows;
public class Farms {
    public static void Draw() {
        ImGui.Text($"Alert me X seconds before a Farm window:");
        var preWarn = Service.Configuration.FarmForewarning.Value;
        ImGui.InputInt("", ref preWarn, 30, 60); 
        if (preWarn != Service.Configuration.FarmForewarning.Value){
            Service.Configuration.FarmForewarning.Value = preWarn;
            Service.Configuration.Save();
        }

        ImGui.Text($"Minimum number of connected windows:");
        var repeatWindows = Service.Configuration.FarmMinDuration.Value;
        ImGui.InputInt("", ref repeatWindows, 1, 1); 
        if (repeatWindows != Service.Configuration.FarmMinDuration.Value && repeatWindows >= 0 && repeatWindows <= 5){
            Service.Configuration.FarmMinDuration.Value = repeatWindows;
            Service.Configuration.Save();
        }

        string test = "";
        foreach (string farmName in Groups.Farms ){
            test += farmName + ", ";
            DrawTools.DrawCheckBox(farmName, Service.Configuration.FarmMinDuration, Service.Configuration.FarmForewarning );
        }
    }
}