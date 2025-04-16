using System.Collections.Generic;
using System.Linq;
using SiteGuard.DataModels.DatabaseSchemas;
using SiteGuard.Webhook;
using SGDiscordWebhook = SiteGuard.DataModels.Discord.DiscordWebhook;

namespace SiteGuard.Modules {
    public static class SeeRecords {
        /// <summary>
        /// Formats a Discord Webhook message and passes it on to WebhookManager.<see cref="WebhookManager.Send"/>.
        /// </summary>
        /// <param name="entries">
        /// List of entries that should be sent with the Discord Webhook.
        /// Automatically caps the List to 10 elements since not more than 10 embeds can be sent with one Discord Webhook.
        /// </param>
        public static void SendDiscordWebhook(List<EntryBase> entries) {
            entries = entries.Take(10).ToList();
            
            SGDiscordWebhook webhook = new SGDiscordWebhook {
                Username = "SiteGuard",
                Embeds = entries.Select(entry => entry.FormatToDiscordEmbed()).ToArray()
            };
            
            WebhookManager.Send(
                Plugin.Instance.Config.ReportWebhookUrl,
                webhook
            );
        }
    }
}