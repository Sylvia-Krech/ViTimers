using ImGuiNET;
using VTimer.Consts;

namespace VTimer.Windows;
public class CurrentTimers {
    public static void Draw() {
        foreach(var T in Service.Trackers){
            T.isUpNextInText();
        }
    }
}