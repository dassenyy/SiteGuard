using System;
using SGDiscordEmbed = SiteGuard.DataModels.Discord.DiscordEmbed;
using SGDiscordEmbedField = SiteGuard.DataModels.Discord.DiscordEmbedField;

namespace SiteGuard.DataModels.DatabaseSchemas {
    public class IdBan : EntryBase {
        public string TargetId { get; set; }
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
                        Value = Plugin.Database.GetUser(TargetId).Name,
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.BanWebhookBannedId,
                        Value = Utility.FormatUserIdForDiscord(TargetId),
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.BanWebhookType,
                        Value = "UserId",
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
                    Plugin.Instance.Config.IdBanEmbedColor
                ),
                Timestamp = IssuedAt
            };
        }
    }
}