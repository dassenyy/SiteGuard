using System;
using Exiled.API.Features;
using SiteGuard.DataModels.DatabaseSchemas;
using SiteGuard.Webhook;
using SGDiscordEmbed = SiteGuard.DataModels.Discord.DiscordEmbed;
using SGDiscordEmbedField = SiteGuard.DataModels.Discord.DiscordEmbedField;
using SGDiscordWebhook = SiteGuard.DataModels.Discord.DiscordWebhook;

namespace SiteGuard.Modules {
    public static class Kicking {
        /// <summary>
        /// Formats a Discord Webhook message and passes it on to WebhookManager.<see cref="WebhookManager.Send"/>.
        /// </summary>
        public static void SendDiscordWebhook(Player issuer, Player target, string reason) {
            SGDiscordWebhook webhook = new SGDiscordWebhook { 
                Username = "SiteGuard",
                Embeds = new[] {
                    new SGDiscordEmbed {
                        Title = $"**{Plugin.Instance.Translation.KickWebhookTitle}**",
                        Fields = new SGDiscordEmbedField[] {
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.KickWebhookIssuerName,
                                Value = issuer.Nickname,
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.KickWebhookIssuerId,
                                Value = Utility.FormatUserIdForDiscord(issuer.UserId),
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.KickWebhookKickedName,
                                Value = target.Nickname,
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.KickWebhookKickedId,
                                Value = Utility.FormatUserIdForDiscord(target.UserId),
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.KickWebhookReason,
                                Value = reason,
                                Inline = false
                            }
                        },
                        Color = Utility.ParseIntHexColor(Plugin.Instance.Config.KickEmbedColor),
                        Timestamp = DateTime.UtcNow
                    }
                }
            };
            
            WebhookManager.Send(
                Plugin.Instance.Config.KickWebhookUrl,
                webhook
            );
        }

        /// <summary>
        /// Inserts the Kick and upserts the involved Users into the database.
        /// </summary>
        public static void InsertIntoDatabase(Player issuer, Player target, string reason) {
            Plugin.Database.UpsertUser(new User { UserId = issuer.UserId, Name = issuer.Nickname });
            Plugin.Database.UpsertUser(new User { UserId = target.UserId, Name = target.Nickname });
            Plugin.Database.InsertKick(new Kick {
                IssuerId = issuer.UserId,
                TargetId = target.UserId,
                Reason = reason,
                IssuedAt = DateTime.UtcNow
            });
        }
    }
}