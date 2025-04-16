using System;
using Exiled.API.Features;
using SiteGuard.DataModels.DatabaseSchemas;
using SiteGuard.Webhook;
using SGDiscordEmbed = SiteGuard.DataModels.Discord.DiscordEmbed;
using SGDiscordEmbedField = SiteGuard.DataModels.Discord.DiscordEmbedField;
using SGDiscordWebhook = SiteGuard.DataModels.Discord.DiscordWebhook;

namespace SiteGuard.Modules {
    public static class Warning {
        /// <summary>
        /// Formats a Discord Webhook message and passes it on to WebhookManager.<see cref="WebhookManager.Send"/>.
        /// </summary>
        /// <remarks>
        /// Since the target player can be null (if the offline warn command was used),
        /// the method will just take info about the target players name and userId instead of from <see cref="Player"/>.
        /// </remarks>
        public static void SendDiscordWebhook(Player issuer, string targetName, string targetId, string reason) {
            SGDiscordWebhook webhook = new SGDiscordWebhook { 
                Username = "SiteGuard",
                Embeds = new[] {
                    new SGDiscordEmbed {
                        Title = $"**{Plugin.Instance.Translation.WarnWebhookTitle}**",
                        Fields = new SGDiscordEmbedField[] {
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.WarnWebhookIssuerName,
                                Value = issuer.Nickname,
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.WarnWebhookIssuerId,
                                Value = Utility.FormatUserIdForDiscord(issuer.UserId),
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.WarnWebhookKickedName,
                                Value = targetName,
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.WarnWebhookKickedId,
                                Value = Utility.FormatUserIdForDiscord(targetId),
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.WarnWebhookReason,
                                Value = reason,
                                Inline = false
                            }
                        },
                        Color = Utility.ParseIntHexColor(Plugin.Instance.Config.WarnEmbedColor),
                        Timestamp = DateTime.UtcNow
                    }
                }
            };
            
            WebhookManager.Send(
                Plugin.Instance.Config.WarnWebhookUrl,
                webhook
            );
        }

        /// <summary>
        /// Inserts the Warn and upserts the involved Users into the database.
        /// </summary>
        /// <remarks>
        /// Since the target player can be null (if the offline warn command was used),
        /// the method will just take info about the target players name and userId instead of from <see cref="Player"/>.
        /// </remarks>
        public static void InsertIntoDatabase(Player issuer, string targetName, string targetId, string reason) {
            Plugin.Database.UpsertUser(new User { UserId = issuer.UserId, Name = issuer.Nickname });
            Plugin.Database.UpsertUser(new User { UserId = targetId, Name = targetName });
            Plugin.Database.InsertWarn(new Warn {
                IssuerId = issuer.UserId,
                TargetId = targetId,
                Reason = reason,
                IssuedAt = DateTime.UtcNow
            });
        }
    }
}