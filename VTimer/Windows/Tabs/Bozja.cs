using ImGuiNET;
using VTimer.Consts;

namespace VTimer.Windows;
public class Bozja {
    public static void Draw(Plugin plugin) {
        foreach(var T in Service.Trackers){
            T.isUpNextInText();
        }
    }
}