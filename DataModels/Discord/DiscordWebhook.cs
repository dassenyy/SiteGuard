using Newtonsoft.Json;

namespace SiteGuard.DataModels.Discord {
    public class DiscordWebhook {
        [JsonProperty("username")]
        public string Username { get; set; } = "SiteGuard";

        [JsonProperty("content")]
        public string Content { get; set; } = "";
        
        [JsonProperty("tts")]
        public bool TTS { get; set; }
        
        [JsonProperty("embeds")]
        public DiscordEmbed[] Embeds { get; set; } = {};
    }
}