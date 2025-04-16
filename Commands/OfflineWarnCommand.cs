using System;
using System.Text.RegularExpressions;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using SiteGuard.Modules;

namespace SiteGuard.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class OfflineWarnCommand : ICommand, IUsageProvider {
        private static readonly Regex _steamIdRegex = new Regex ("^\\d{17}@steam$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        
        public string Command => "owarn";
        public string[] Aliases { get; } = { };
        public string Description => Plugin.Instance.Translation.OfflineWarnCommandDescription;
        public string[] Usage { get; } = {
            Plugin.Instance.Translation.CommandArgumentOfflinePlayer,
            Plugin.Instance.Translation.CommandArgumentReason
        };
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            // Make sure that:
            // - Player has permissions
            // - All arguments are provided
            // - SteamId (Argument 1) is in a valid format
            
            Player issuer = Player.Get(sender);
            if (!issuer.CheckPermission("siteguard.warn")) {
                response = Plugin.Instance.Translation.CommandMissingPermissions;
                return false;
            }
            
            if (arguments.Count < 2) {
                response = Plugin.Instance.Translation.CommandNotEnoughArguments;
                return false;
            }
            
            string targetId = arguments.At(0);
            if (Utility.IsSteamIdValidFormat(targetId)) { 
                response = Plugin.Instance.Translation.CommandWrongSteamIdFormat;
                return false;
            }
            
            string reason = string.Join(" ", arguments.Array, 2, arguments.Count - 1);
            
            if (!string.IsNullOrWhiteSpace(Plugin.Instance.Config.WarnWebhookUrl)) {
                Warning.SendDiscordWebhook(issuer, "Unknown - offline", targetId, reason);
            }
            
            response = Plugin.Instance.Translation.WarnCommandSuccess;
            return true;
        }
    }
}