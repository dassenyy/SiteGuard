using System;
using Newtonsoft.Json;

namespace SiteGuard.DataModels.Discord {
    public class DiscordEmbed {
        [JsonProperty("title")]
        public string Title { get; set; } = "Empty Title";
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("color")]
        public int Color { get; set; }

        [JsonProperty("footer")]
        public DiscordEmbedFooter Footer { get; set; }
        
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
        
        [JsonProperty("fields")]
        public DiscordEmbedField[] Fields { get; set; }
    }
    
    public class DiscordEmbedField {
        [JsonProperty("name")]
        public string Name { get; set; } = "";
        
        [JsonProperty("value")]
        public string Value { get; set; } = "";
        
        [JsonProperty("inline")]
        public bool Inline { get; set; }
    }

    public class DiscordEmbedFooter {
        [JsonProperty("text")]
        public string Text { get; set; } = "";
    }
}