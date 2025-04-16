using System;
using Exiled.API.Features;
using SiteGuard.DataModels.DatabaseSchemas;
using SiteGuard.Webhook;
using Server = Exiled.API.Features.Server;
using SGDiscordEmbed = SiteGuard.DataModels.Discord.DiscordEmbed;
using SGDiscordEmbedField = SiteGuard.DataModels.Discord.DiscordEmbedField;
using SGDiscordWebhook = SiteGuard.DataModels.Discord.DiscordWebhook;

namespace SiteGuard.Modules {
    public static class Reporting {
        /// <summary>
        /// Formats a Discord Webhook message and passes it on to WebhookManager.<see cref="WebhookManager.Send"/>.
        /// </summary>
        public static void SendDiscordWebhook(Player reporter, Player reported, string reason) {
            SGDiscordWebhook webhook = new SGDiscordWebhook { 
                Username = "SiteGuard",
                Embeds = new[] {
                    new SGDiscordEmbed {
                        Title = $"**{Plugin.Instance.Translation.ReportWebhookTitle}**",
                        Fields = new SGDiscordEmbedField[] {
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.ReportWebhookServerName,
                                Value = Utility.FormatServerName(),
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.ReportWebhookServerEndpoint,
                                Value = $"`{Server.IpAddress}:{Server.Port}`",
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.ReportWebhookReporterName,
                                Value = reporter.Nickname,
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.ReportWebhookReporterId,
                                Value = Utility.FormatUserIdForDiscord(reporter.UserId),
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.ReportWebhookReportedName,
                                Value = reported.Nickname,
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.ReportWebhookReportedId,
                                Value = Utility.FormatUserIdForDiscord(reported.UserId),
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.ReportWebhookReason,
                                Value = reason,
                                Inline = false
                            }
                        },
                        Color = Utility.ParseIntHexColor(Plugin.Instance.Config.ReportEmbedColor),
                        Timestamp = DateTime.UtcNow
                    }
                }
            };

            WebhookManager.Send(
                Plugin.Instance.Config.ReportWebhookUrl,
                webhook
            );
        }

        /// <summary>
        /// Inserts the Report and upserts the involved Users into the database.
        /// </summary>
        public static void InsertIntoDatabase(Player reporter, Player reported, string reason) {
            Plugin.Database.UpsertUser(new User { UserId = reporter.UserId, Name = reporter.Nickname });
            Plugin.Database.UpsertUser(new User { UserId = reported.UserId, Name = reported.Nickname });
            Plugin.Database.InsertReport(new Report {
                IssuerId = reporter.UserId,
                TargetId = reported.UserId,
                Reason = reason,
                IssuedAt = DateTime.UtcNow,
            });
        }
    }
}