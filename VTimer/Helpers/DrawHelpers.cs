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

}
