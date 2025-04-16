using Exiled.Events.EventArgs.Server;
using SiteGuard.Modules;

namespace SiteGuard.EventHandlers {
    public static class ServerHandler {
        /// <inheritdoc cref="Exiled.Events.Handlers.Server.OnLocalReporting"/>
        public static void OnLocalReporting(LocalReportingEventArgs eventArguments) {
            if (eventArguments.IsAllowed && !string.IsNullOrWhiteSpace(Plugin.Instance.Config.ReportWebhookUrl)) {
                Reporting.SendDiscordWebhook(eventArguments.Player, eventArguments.Target, eventArguments.Reason);
            }
            
            Reporting.InsertIntoDatabase(eventArguments.Player, eventArguments.Target, eventArguments.Reason);
        }
    }
}