using System;
using SGDiscordEmbed = SiteGuard.DataModels.Discord.DiscordEmbed;

namespace SiteGuard.DataModels.DatabaseSchemas {
    public abstract class EntryBase {
        public string IssuerId { get; set; }
        public string Reason { get; set; }
        public DateTime IssuedAt { get; set; }
        
        public abstract SGDiscordEmbed FormatToDiscordEmbed();
    }
}