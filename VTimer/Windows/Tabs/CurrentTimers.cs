using ImGuiNET;
using VTimer.Consts;

namespace VTimer.Windows;
public class CurrentTimers {
    public static void Draw(Plugin plugin) {
        foreach(var T in Service.Trackers){
            ImGui.Text(T.name + " is up next in " + Service.ETM.delayToTime(T.getUpcommingWindow() - Service.ETM.now()));
        }
    }
}