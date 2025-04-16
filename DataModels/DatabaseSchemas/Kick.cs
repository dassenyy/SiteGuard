using SGDiscordEmbed = SiteGuard.DataModels.Discord.DiscordEmbed;
using SGDiscordEmbedField = SiteGuard.DataModels.Discord.DiscordEmbedField;

namespace SiteGuard.DataModels.DatabaseSchemas {
    public class Kick : EntryBase {
        public string TargetId { get; set; }
        
        public override SGDiscordEmbed FormatToDiscordEmbed() {
            return new SGDiscordEmbed {
                Title = $"**{Plugin.Instance.Translation.KickWebhookTitle}**",
                Fields = new SGDiscordEmbedField[] {
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.KickWebhookIssuerName,
                        Value = Plugin.Database.GetUser(IssuerId).Name,
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.KickWebhookIssuerId,
                        Value = Utility.FormatUserIdForDiscord(IssuerId),
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.KickWebhookKickedName,
                        Value = Plugin.Database.GetUser(TargetId).Name,
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.KickWebhookKickedId,
                        Value = Utility.FormatUserIdForDiscord(TargetId),
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.KickWebhookReason,
                        Value = Reason,
                        Inline = false
                    }
                },
                Color = Utility.ParseIntHexColor(Plugin.Instance.Config.KickEmbedColor),
                Timestamp = IssuedAt
            };
        }
    }
}