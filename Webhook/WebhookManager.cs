using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Newtonsoft.Json;
using SGDiscordWebhook = SiteGuard.DataModels.Discord.DiscordWebhook;

namespace SiteGuard.Webhook {
    public static class WebhookManager {
        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Sends a Webhook to the provided URL. If it fails it logs the error to the LocalAdmin console.
        /// </summary>
        public static void Send(string webhookURL, SGDiscordWebhook webhook) {
            StringContent content = new StringContent(
                JsonConvert.SerializeObject(webhook),
                Encoding.UTF8,
                "application/json"
            );
            
            Task.Run(async () => {
                try {
                    HttpResponseMessage response = await _httpClient.PostAsync(webhookURL, content);
                    response.EnsureSuccessStatusCode();
                } catch (Exception e) {
                    Log.Error(e);
                }
            });
        }
    }
}