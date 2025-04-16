using System;
using System.IO;
using Exiled.API.Enums;
using Exiled.API.Features;
using SiteGuard.Database;
using SiteGuard.EventHandlers;
using ExiledPlayerHandler = Exiled.Events.Handlers.Player;
using ExiledServerHandler = Exiled.Events.Handlers.Server;

namespace SiteGuard {
    public class Plugin : Plugin<Config, Translation> {
        public static Plugin Instance { get; private set; }
        public static SGDatabase Database { get; private set; }
        
        public override string Name => "SiteGuard";
        public override string Prefix => "siteguard";
        public override string Author => "dassenyy";
        public override PluginPriority Priority => PluginPriority.Default;
        public override Version Version { get; } = new Version(1, 0, 0);
        
        public override void OnEnabled() {
            Instance = this;
            
            Directory.CreateDirectory(Path.Combine(Paths.Exiled, "SiteGuard"));
            Database = new SGDatabase($"Filename={Path.Combine(Paths.Exiled, "SiteGuard", "data.db")};");
            
            ExiledServerHandler.LocalReporting += ServerHandler.OnLocalReporting;
            ExiledPlayerHandler.Kicking += PlayerHandler.OnKicking;
            ExiledPlayerHandler.Banned += PlayerHandler.OnBanned;
            
            base.OnEnabled();
        }

        public override void OnDisabled() {
            ExiledServerHandler.LocalReporting -= ServerHandler.OnLocalReporting;
            ExiledPlayerHandler.Kicking -= PlayerHandler.OnKicking;
            ExiledPlayerHandler.Banned -= PlayerHandler.OnBanned;
            
            Database = null;
            
            Instance = null;
            
            base.OnDisabled();
        }
    }
}