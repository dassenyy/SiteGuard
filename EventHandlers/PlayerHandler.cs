using Exiled.Events.EventArgs.Player;
using SiteGuard.Modules;

namespace SiteGuard.EventHandlers {
    public static class PlayerHandler {
        /// <inheritdoc cref="Exiled.Events.Handlers.Player.Banned"/>
        public static void OnBanned(BannedEventArgs eventArguments) {
            if (!string.IsNullOrWhiteSpace(Plugin.Instance.Config.BanWebhookUrl)) {
                Banning.SendDiscordWebhook(eventArguments.Player, eventArguments.Details, eventArguments.Type);
            }
            
            Banning.InsertIntoDatabase(eventArguments.Player, eventArguments.Details, eventArguments.Type);
        }
        
        /// <inheritdoc cref="Exiled.Events.Handlers.Player.Kicking"/>
        public static void OnKicking(KickingEventArgs eventArguments) {
            if (eventArguments.IsAllowed && !string.IsNullOrWhiteSpace(Plugin.Instance.Config.KickWebhookUrl)) {
                Kicking.SendDiscordWebhook(eventArguments.Player, eventArguments.Target, eventArguments.Reason);
            }
            
            Kicking.InsertIntoDatabase(eventArguments.Player, eventArguments.Target, eventArguments.Reason);
        }
    }
}