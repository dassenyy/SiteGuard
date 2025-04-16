using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using SiteGuard.Modules;

namespace SiteGuard.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class WarnCommand : ICommand, IUsageProvider {
        public string Command => "warn";
        public string[] Aliases { get; } = { };
        public string Description => Plugin.Instance.Translation.WarnCommandDescription;
        public string[] Usage { get; } = {
            Plugin.Instance.Translation.CommandArgumentOnlinePlayer, 
            Plugin.Instance.Translation.CommandArgumentReason
        };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            // Make sure that:
            // - Player has permissions
            // - All arguments are provided
            // - PlayerId or Name (Argument 1) can be parsed to Exiled.API.Features.Player
            
            Player issuer = Player.Get(sender);
            if (!issuer.CheckPermission("siteguard.warn")) {
                response = Plugin.Instance.Translation.CommandMissingPermissions;
                return false;
            }
            
            if (arguments.Count < 2) {
                response = Plugin.Instance.Translation.CommandNotEnoughArguments;
                return false;
            }

            Player target = Int32.TryParse(arguments.At(0), out int playerId) ? Player.Get(playerId) : Player.Get(arguments.At(0));
            if (target == null) {
                response = Plugin.Instance.Translation.CommandPlayerNotFound;
                return false;
            }
            
            string reason = string.Join(" ", arguments.Array, 2, arguments.Count - 1);
            
            if (!string.IsNullOrWhiteSpace(Plugin.Instance.Config.WarnWebhookUrl)) {
                Warning.SendDiscordWebhook(issuer, target.Nickname, target.UserId, reason);
            }
            
            Warning.InsertIntoDatabase(issuer, target.Nickname, target.UserId, reason);
            
            response = Plugin.Instance.Translation.WarnCommandSuccess;
            return true;
        }
    }
}