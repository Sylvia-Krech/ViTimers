using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Internal.Windows.Settings.Widgets;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using VTimer.Consts;
using VTimer.Helpers;

namespace VTimer.Windows;

public class TimedEvents {
    static long VERMINION_OFFSET = 486000;
    static long minute = 60;
    static long hour = 60 * 60;
    static long day = hour * 24;
    static long week = day * 7;

    public static void Draw() {
        var time = EorzeanTime.now();
        foreach (string name in Groups.realTime ){
            DrawTools.DrawCheckBox(name, Numbers.ZeroVal, Service.Configuration.RealForewarning);
        }
        return;
        // Ocean fishing boats and open tournaments happen on the hour, cycling between the two
        var boat = time % (60*60*2);
        if (boat > 60*60){
            ImGui.TextColored(Colors.upSoon, "Ocean Fishing will be available in " + EorzeanTime.delayToTimeText((hour * 2) - boat));
            if (boat < 60*60 + (60*30)) {
                ImGui.TextColored(Colors.up, "TT Open Tournament is available for " + EorzeanTime.delayToTimeText((hour) - boat - (60*30)));
            } else {
                ImGui.TextColored(Colors.upEventually, "TT Open Tournament will be available in " + EorzeanTime.delayToTimeText((hour) - boat));
            }
        }
        else {
            ImGui.TextColored(Colors.upSoon, "TT Open Tournament will be available in " + EorzeanTime.delayToTimeText((hour) - boat));
            if (boat < (60*15)) {
                ImGui.TextColored(Colors.upSoon, "Ocean Fishing is available for " + EorzeanTime.delayToTimeText((hour * 2) - boat - (60*15)));
            } 
            else {
                ImGui.TextColored(Colors.upEventually, "Ocean Fishing will be available in " + EorzeanTime.delayToTimeText((hour * 2) - boat));
            }
        }

        //TT bi-weekly tournaments
        var biweekly = (time-VERMINION_OFFSET) % (week * 2);
        if ( biweekly < week ) {
            ImGui.TextColored(Colors.up, "TT Biweekly Tournament is available for " + EorzeanTime.delayToTimeText((week) - biweekly));
        }
        else {
            ImGui.TextColored(Colors.upEventually, "TT Biweekly Tournament will be available in " + EorzeanTime.delayToTimeText((week * 2) - biweekly));
        }

        //Fashion Report
        var fashionReport = (time-VERMINION_OFFSET) % week;
        if (fashionReport < (day * 3)) {
            ImGui.TextColored(Colors.upEventually, "Fashion Report will be available in " + EorzeanTime.delayToTimeText((day * 3) - fashionReport));
        }
        else {
            ImGui.TextColored(Colors.up, "Fashion Report is available for " + EorzeanTime.delayToTimeText((week) - fashionReport));
        }
    }

    
}
