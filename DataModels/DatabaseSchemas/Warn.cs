using SGDiscordEmbed = SiteGuard.DataModels.Discord.DiscordEmbed;
using SGDiscordEmbedField = SiteGuard.DataModels.Discord.DiscordEmbedField;

namespace SiteGuard.DataModels.DatabaseSchemas {
    public class Warn : EntryBase {
        public string TargetId { get; set; }
        
        public override SGDiscordEmbed FormatToDiscordEmbed() {
            return new SGDiscordEmbed {
                Title = $"**{Plugin.Instance.Translation.WarnWebhookTitle}**",
                Fields = new SGDiscordEmbedField[] {
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.WarnWebhookIssuerName,
                        Value = Plugin.Database.GetUser(IssuerId).Name,
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.WarnWebhookIssuerId,
                        Value = Utility.FormatUserIdForDiscord(IssuerId),
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.WarnWebhookKickedName,
                        Value = Plugin.Database.GetUser(TargetId).Name,
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.WarnWebhookKickedId,
                        Value = Utility.FormatUserIdForDiscord(TargetId),
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.WarnWebhookReason,
                        Value = Reason,
                        Inline = false
                    }
                },
                Color = Utility.ParseIntHexColor(Plugin.Instance.Config.WarnEmbedColor),
                Timestamp = IssuedAt
            };
        }
    }
}