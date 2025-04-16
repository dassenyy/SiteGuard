using System;
using SGDiscordEmbed = SiteGuard.DataModels.Discord.DiscordEmbed;
using SGDiscordEmbedField = SiteGuard.DataModels.Discord.DiscordEmbedField;

namespace SiteGuard.DataModels.DatabaseSchemas {
    public class Report : EntryBase {
        public string TargetId { get; set; }
        public DateTime Test { get; set; }
        
        public override SGDiscordEmbed FormatToDiscordEmbed() {
            return new SGDiscordEmbed {
                Title = $"**{Plugin.Instance.Translation.ReportWebhookTitle}**",
                Fields = new SGDiscordEmbedField[] {
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.ReportWebhookReporterName,
                        Value = Plugin.Database.GetUser(IssuerId).Name,
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.ReportWebhookReporterId,
                        Value = Utility.FormatUserIdForDiscord(IssuerId),
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.ReportWebhookReportedName,
                        Value = Plugin.Database.GetUser(TargetId).Name,
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.ReportWebhookReportedId,
                        Value = Utility.FormatUserIdForDiscord(TargetId),
                        Inline = true
                    },
                    new SGDiscordEmbedField {
                    },
                    new SGDiscordEmbedField {
                        Name = Plugin.Instance.Translation.ReportWebhookReason,
                        Value = Reason,
                        Inline = false
                    }
                },
                Color = Utility.ParseIntHexColor(Plugin.Instance.Config.ReportEmbedColor),
                Timestamp = IssuedAt
            };
        }
    }
}