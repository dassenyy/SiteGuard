using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using SiteGuard.Modules;

namespace SiteGuard.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class MockReportCommand : ICommand, IUsageProvider {
        public string Command => "mockreport";
        public string[] Aliases { get; } = { "mreport" };
        public string Description => "Mocks a report for testing purposes.";
        public string[] Usage { get; } = {
            Plugin.Instance.Translation.CommandArgumentReason
        };
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            Player issuer = Player.Get(sender);
            if (!issuer.CheckPermission("siteguard.dev")) {
                response = Plugin.Instance.Translation.CommandMissingPermissions;
                return false;
            }
            
            if (arguments.Count < 1) {
                response = Plugin.Instance.Translation.CommandNotEnoughArguments;
                return false;
            }
            
            string reason = string.Join(" ", arguments.Array, 1, arguments.Count);
            
            if (!string.IsNullOrWhiteSpace(Plugin.Instance.Config.WarnWebhookUrl)) {
                Reporting.SendDiscordWebhook(issuer, issuer, reason);
            }
            
            Reporting.InsertIntoDatabase(issuer, issuer, reason);
            
            response = "Mock report successful.";
            return true;
        }
    }
}