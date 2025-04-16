using System;
using Exiled.API.Features;
using SiteGuard.DataModels.DatabaseSchemas;
using SiteGuard.Webhook;
using SGDiscordEmbed = SiteGuard.DataModels.Discord.DiscordEmbed;
using SGDiscordEmbedField = SiteGuard.DataModels.Discord.DiscordEmbedField;
using SGDiscordWebhook = SiteGuard.DataModels.Discord.DiscordWebhook;

namespace SiteGuard.Modules {
    public static class Banning {
        /// <summary>
        /// Formats a Discord Webhook message and passes it on to WebhookManager.<see cref="WebhookManager.Send"/>.
        /// </summary>
        /// <remarks>
        /// Since the target player in the <see cref="Exiled.Events.EventArgs.Player.BannedEventArgs"/> can be null (if it's an offline ban),
        /// the method will take info about the target player from the ban details instead of from <see cref="Player"/>.
        /// </remarks>
        public static void SendDiscordWebhook(Player issuer, BanDetails banDetails, BanHandler.BanType banType) {
            SGDiscordWebhook webhook = new SGDiscordWebhook { 
                Username = "SiteGuard",
                Embeds = new[] {
                    new SGDiscordEmbed {
                        Title = $"**{Plugin.Instance.Translation.BanWebhookTitle}**",
                        Fields = new SGDiscordEmbedField[] {
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.BanWebhookIssuerName,
                                Value = issuer.Nickname,
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.BanWebhookIssuerId,
                                Value = Utility.FormatUserIdForDiscord(issuer.UserId),
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.BanWebhookBannedName,
                                Value = banDetails.OriginalName,
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                                Name = (
                                    banType == BanHandler.BanType.UserId
                                    ? Plugin.Instance.Translation.BanWebhookBannedId
                                    : Plugin.Instance.Translation.BanWebhookBannedIp
                                ),
                                Value = (
                                    banType == BanHandler.BanType.UserId
                                    ? Utility.FormatUserIdForDiscord(banDetails.Id)
                                    : banDetails.Id
                                ),
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.BanWebhookType,
                                Value = banType.ToString(),
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.BanWebhookLength,
                                Value = Utility.FormatUnixTimeToDuration(
                                    (banDetails.Expires - banDetails.IssuanceTime) / 10_000_000L
                                ),
                                Inline = true
                            },
                            new SGDiscordEmbedField {
                            },
                            new SGDiscordEmbedField {
                                Name = Plugin.Instance.Translation.BanWebhookReason,
                                Value = banDetails.Reason,
                                Inline = false
                            }
                        },
                        Color = Utility.ParseIntHexColor(
                            banType == 0 ? Plugin.Instance.Config.IdBanEmbedColor : Plugin.Instance.Config.IpBanEmbedColor
                        ),
                        Timestamp = new DateTime(banDetails.IssuanceTime, DateTimeKind.Utc)
                    }
                }
            };
            
            WebhookManager.Send(
                Plugin.Instance.Config.BanWebhookUrl,
                webhook
            );
        }

        /// <summary>
        /// Inserts either the IdBan or IPBan and upserts the involved Users into the database.
        /// </summary>
        /// <remarks>
        /// Since the target player in the <see cref="Exiled.Events.EventArgs.Player.BannedEventArgs"/> can be null (if it's an offline ban),
        /// the method will take info about the target player from the ban details instead of from <see cref="Player"/>.
        /// </remarks>
        public static void InsertIntoDatabase(Player issuer, BanDetails banDetails, BanHandler.BanType banType) {
            if (banType == BanHandler.BanType.UserId) {
                Plugin.Database.UpsertUser(new User { UserId = issuer.UserId, Name = issuer.Nickname });
                Plugin.Database.UpsertUser(new User { UserId = banDetails.Id, Name = banDetails.OriginalName });
                Plugin.Database.InsertIdBan(new IdBan {
                    IssuerId = issuer.UserId,
                    TargetId = banDetails.Id,
                    Reason = banDetails.Reason,
                    IssuedAt = new DateTime(banDetails.IssuanceTime, DateTimeKind.Utc),
                    ExpiresAt = new DateTime(banDetails.Expires, DateTimeKind.Utc)
                });
            } else {
                Plugin.Database.InsertIPBan(new IPBan {
                    IssuerId = issuer.UserId,
                    TargetIP = banDetails.Id,
                    Reason = banDetails.Reason,
                    IssuedAt = new DateTime(banDetails.IssuanceTime, DateTimeKind.Utc),
                    ExpiresAt = new DateTime(banDetails.Expires, DateTimeKind.Utc)
                });
            }
        }
    }
}