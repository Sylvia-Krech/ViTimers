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

namespace VTimer
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "VTimer";
        private long counter = 0;
        private const string CommandName = "/vtimer";

        private DalamudPluginInterface PluginInterface { get; init; }
        private ICommandManager CommandManager { get; init; }
        //public PluginConfiguration Configuration { get; init; }
        public WindowSystem WindowSystem = new("VTimer");

        public EorzeanTimeManager ETM = new EorzeanTimeManager();

        public MainWindow MainWindow { get; init; }

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ICommandManager commandManager
            )
        {
            _ = pluginInterface.Create<Service>();
            Service.Plugin = this;

            this.PluginInterface = pluginInterface;
            Service.CommandManager = commandManager;

            Service.Configuration = this.PluginInterface.GetPluginConfig() as PluginConfiguration ?? new PluginConfiguration();
            Service.Configuration.Initialize(this.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream
            // var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            // var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            MainWindow = new MainWindow(this);
            
            WindowSystem.AddWindow(MainWindow);

            Service.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp, such as this"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

            Consts.PresetTimers.LoadTimers();

            Service.Framework.Update += onUpdate;
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            
            MainWindow.Dispose();
            
            Service.CommandManager.RemoveHandler(CommandName);
            Service.Framework.Update -= onUpdate;
            Service.Trackers = new();
        }

        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            MainWindow.IsOpen = true;
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            MainWindow.IsOpen = true;
        }

        //Core part of the program, runs every frame to my understanding.
        public void onUpdate(IFramework framework) {
            counter += 1;
            if (counter % 60 == 0) {
                var now = Service.ETM.now();
                foreach (Tracker tracker in Service.Trackers) {
                    Service.PluginLog.Verbose(tracker.name + " next window: " + tracker.getNextWindowInQueue()%10000 + " now + forewarning: " + (now+tracker.getForewarning()) %10000);
                    if (tracker.getNextWindowInQueue() < now + tracker.getForewarning() ) {
                        Service.PluginLog.Verbose("Notifying that " + tracker.name + " is up in " + (tracker.getNextWindowInQueue() - now)  + " seconds");
                        tracker.notify();
                        tracker.recycle();
                    }
                }
            }
        }
    }
}
