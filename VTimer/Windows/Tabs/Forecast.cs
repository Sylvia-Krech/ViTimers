using ImGuiNET;
using VTimer.Consts;
using VTimer.Helpers;

namespace VTimer.Windows;
public class Forecast {
    public static void Draw() {
        //Service.ClosestWindows.Sort();
        if (ImGui.BeginTable("table1", 2))
        {
            const int MaxRows = 20;
            int rowCount = System.Math.Min(MaxRows, Service.ClosestWindows.Count);
            for (int row = 0; row < rowCount; row++)
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);

                Timestamp ts = Service.ClosestWindows[row];

                DrawTools.DrawWindowPair(ts);
            }
            ImGui.EndTable();
        }
    }

}