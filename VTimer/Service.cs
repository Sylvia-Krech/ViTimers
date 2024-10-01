using System.Collections.Generic;
//using Dalamud.ContextMenu;
using Dalamud.Game;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using VTimer.Helpers;

namespace VTimer;

internal class Service
{
    internal static readonly string Version = "v0.1";
    internal static Plugin Plugin { get; set; } = null!;
    //internal static PluginWindow PluginUi { get; set; } = null!;
    //internal static SettingsWindow SettingsUi { get; set; } = null!;
    internal static PluginConfiguration Configuration { get; set; } = null!;
    //internal static DalamudContextMenu ContextMenu { get; set; } = null!; 
    //internal static Ipc Ipc { get; set; } = null!;

    internal static List<Tracker> Trackers { get; set; } = new();
    internal static List<Tracker> CustomTrackers { get; set; } = new();
    
    //must be sorted whenever things are added I guess, because SortedList isn't actually a goddamn list, and is functionally a dictionary
    internal static List<Timestamp> ClosestWindows { get; set; } = new(); 


    [PluginService] public static IFramework Framework { get; set; } = null!;
    [PluginService] public static IChatGui Chat { get; set; } = null!;
    [PluginService] public static IClientState ClientState { get; set; } = null!;
    [PluginService] public static ICommandManager CommandManager { get; set; } = null!;
    [PluginService] public static IDataManager DataManager { get; set; } = null!;
    [PluginService] public static IGameGui GameGui { get; set; } = null!;
    [PluginService] public static IDalamudPluginInterface Interface { get; set; } = null!;
    [PluginService] public static ISigScanner SigScanner { get; set; } = null!;
    [PluginService] public static IKeyState KeyState { get; set; } = null!;
    [PluginService] public static IPluginLog PluginLog { get; set; } = null!;
}