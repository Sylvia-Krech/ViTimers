using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using VTimer.Windows;
using VTimer.Helpers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using FFXIVClientStructs.FFXIV.Component.Shell;
using System.Collections.Generic;
using System.Linq;

namespace VTimer
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "VTimer";
        private long counter = 0;
        public bool filledQueues = false;
        private const string MainCommandName = "/vtimer";
        private const string ConfigCommandName = "/vtimerconfig";

        private IDalamudPluginInterface PluginInterface { get; init; }
        //public PluginConfiguration Configuration { get; init; }
        public WindowSystem WindowSystem = new("VTimer");

        public ConfigWindow ConfigWindow { get; init; } 
        public ForecastWindow ForecastWindow { get; init; } 

        public Plugin(
            IDalamudPluginInterface pluginInterface,
            ICommandManager commandManager
            )
        {
            _ = pluginInterface.Create<Service>();
            Service.Plugin = this;

            Service.PluginLog.Verbose("Loading VTimer...2 ");
            Service.PluginLog.Verbose("Current time: " + Helpers.EorzeanTime.getCurrentEorzeanTime() );
            Service.PluginLog.Verbose("Current weather number: " + Helpers.EorzeanTime.getCurrentWeatherNumber() );

            PluginInterface = pluginInterface;
            Service.CommandManager = commandManager;

            Service.Configuration = PluginInterface.GetPluginConfig() as PluginConfiguration ?? new PluginConfiguration();
            Service.Configuration.Initialize(PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream
            // var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            // var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);
            
            ConfigWindow = new ConfigWindow();
            ForecastWindow = new ForecastWindow();

            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(ForecastWindow);

            Service.CommandManager.AddHandler(MainCommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "To view upcomming windows\nconfig to access the settings\net for calculating unix time from eorzean"
            });

            PluginInterface.UiBuilder.Draw += DrawUI;
            PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

            Helpers.PresetTimers.LoadTimers();

            Service.Framework.Update += onUpdate;
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            
            ConfigWindow.Dispose();
            
            Service.CommandManager.RemoveHandler(MainCommandName);
            Service.Framework.Update -= onUpdate;
            Service.Trackers = new();
        }

        private void OnCommand(string command, string arguments)
        {
            Service.PluginLog.Verbose("Command|" + command + "|    args|" + arguments +"|");
            List<string> args = arguments.Split(" ").ToList();
            string mainArg = args[0].ToLower();
            if (command == MainCommandName){
                //Config window
                if (mainArg == "config" || mainArg == "c") {
                    Service.PluginLog.Verbose("Toggling config window");
                    ConfigWindow.IsOpen = !ConfigWindow.IsOpen;
                } 

                //Forecast window
                else if (mainArg == "") {
                    Service.PluginLog.Verbose("Toggling main window");
                    ForecastWindow.IsOpen = !ForecastWindow.IsOpen;
                }

                //Unix command
                else if (mainArg == "et") {
                    Service.PluginLog.Verbose("Getting Eorzean Time");
                    List<long> inputs = new List<long>();
                    args.RemoveAt(0); //remove "et"
                    foreach (string s in args){
                        inputs.Add(long.Parse(s));
                    }
                    if (inputs.Count() < 1) {
                        Service.PluginLog.Warning("No time provided");
                        return;
                    }
                    if (inputs.Count() < 2) {
                        inputs.Add(0);
                    }
                    if (inputs.Count() < 3) {
                        inputs.Add(0);
                    }
                    long output = EorzeanTime.GetEorzeanAsUnix(inputs[0], inputs[1], inputs[2], EorzeanTime.now());
                    if (output != -1) {
                        Service.Chat.Print("" + output);
                    }                    
                } 
            } 
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }

        public void onUpdate(IFramework framework) {
            counter += 1;
            if (counter % 60 == 0) {
                foreach (Tracker tracker in Service.Trackers) {
                    //Service.PluginLog.Verbose(tracker.name + " is " + tracker.upcommingWindowStatus());
                    switch (tracker.upcommingWindowStatus()) {
                        case TimestampStatus.upSoon:
                            Service.PluginLog.Verbose("Notifying " + tracker.name);
                            tracker.notify();
                            break;
                        case TimestampStatus.past:
                            Service.PluginLog.Verbose("Recycling " + tracker.name);
                            tracker.recycle();
                            break;
                        default:
                            break;
                    }
                }
            } else if (!filledQueues) {
                var now = EorzeanTime.now();
                bool madeNewTimestamp = false;
                foreach (Tracker tracker in Service.Trackers) {
                    if (tracker.numberOfWindowsInQueue() <  Consts.Numbers.MaxWindowsToPreload && now + Consts.Numbers.MaxOutlook > tracker.endOfLastWindow() ) {
                        Service.PluginLog.Verbose("num in queue before: " + tracker.numberOfWindowsInQueue());
                        tracker.findAnotherWindow();
                        Service.PluginLog.Verbose("num in queue after: " + tracker.numberOfWindowsInQueue());
                        madeNewTimestamp = true;
                        break;
                    }
                }
                if (!madeNewTimestamp) {
                    filledQueues = true;
                }
            }
        }
    }
}
