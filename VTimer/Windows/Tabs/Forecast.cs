using System.Collections.Generic;
using System.Linq;
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
            List<Timestamp> windows = new List<Timestamp>();
            foreach (Tracker T in Service.Trackers) {
                windows.AddRange(T.nextWindows);
            }
            int rowCount = System.Math.Min(MaxRows, windows.Count);
            windows.Sort();
            for (int row = 0; row < rowCount; row++)
            {
                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);

                Timestamp ts = windows[row];

                DrawTools.DrawWindowPair(ts);
            }
            ImGui.EndTable();
        }
    }

}