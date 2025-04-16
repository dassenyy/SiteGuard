using System;
using SGDiscordEmbed = SiteGuard.DataModels.Discord.DiscordEmbed;
using SGDiscordEmbedField = SiteGuard.DataModels.Discord.DiscordEmbedField;

namespace SiteGuard.DataModels.DatabaseSchemas {
    public class IPBan : EntryBase {
        public string TargetIP { get; set; }
        public DateTime ExpiresAt { get; set; }
        
        public override SGDiscordEmbed FormatToDiscordEmbed() {
            return new SGDiscordEmbed {
                Title = $"**{Plugin.Instance.Translation.BanWebhookTitle}**",
                Fields = new SGDiscordEmbedField[] {
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.BanWebhookIssuerName,
                        Value = Plugin.Database.GetUser(IssuerId).Name,
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.BanWebhookIssuerId,
                        Value = Utility.FormatUserIdForDiscord(IssuerId),
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.BanWebhookBannedName,
                        Value = "Unknown - IP ban",
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                        Name =Plugin.Instance.Translation.BanWebhookBannedIp,
                        Value = TargetIP,
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.BanWebhookType,
                        Value = "IP",
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.BanWebhookLength,
                        Value = Utility.FormatUnixTimeToDuration(
                            (ExpiresAt.Ticks - IssuedAt.Ticks) / 10_000_000L
                        ),
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.BanWebhookReason,
                        Value = Reason,
                        Inline = false
                    }
                },
                Color = Utility.ParseIntHexColor(
                    Plugin.Instance.Config.IpBanEmbedColor
                ),
                Timestamp = IssuedAt
            };
        }
    }
}