using System;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using SiteGuard.DataModels.DatabaseSchemas;
using SiteGuard.Modules;

namespace SiteGuard.Commands {
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SeeRecordsCommand : ICommand, IUsageProvider {
        public string Command => "seerecords";
        public string[] Aliases { get; } = { "crecords" };
        public string Description => Plugin.Instance.Translation.SeeRecordsCommandDescription;
        public string[] Usage { get; } = {
            Plugin.Instance.Translation.CommandArgumentOnlinePlayer,
            Plugin.Instance.Translation.CommandArgumentSeeRecordsLimit,
            Plugin.Instance.Translation.CommandArgumentSeeRecordsTypes
        };
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response) {
            // Make sure that:
            // - Player has permissions
            // - There is a URL configured for the SeeRecordsWebhookUrl key (if not nothing can be sent so it won't make sense to continue)
            // - All arguments are provided
            // - PlayerId or Name (Argument 1) can be parsed to an Exiled.API.Features.Player
            // - Limit (Argument 2) can be parsed to an int,
            //   and set it to 10 (can't have more than 10 embeds in a Discord Webhook) if it's above 10 or below 1
            // - List of entries actually contains at least one entry for the target player and selected entry types 
            
            Player issuer = Player.Get(sender);
            if (!issuer.CheckPermission("siteguard.seenotes")) {
                response = Plugin.Instance.Translation.CommandMissingPermissions;
                return false;
            }
            
            if (string.IsNullOrWhiteSpace(Plugin.Instance.Config.SeeRecordsWebhookUrl)) {
                response = Plugin.Instance.Translation.SeeRecordsCommandNoWebhook;
                return false;
            }

            if (arguments.Count < 3) {
                response = Plugin.Instance.Translation.CommandNotEnoughArguments;
                return false;
            }
            
            Player target = Int32.TryParse(arguments.At(0), out int playerId) ? Player.Get(playerId) : Player.Get(arguments.At(0));
            if (target == null) {
                response = Plugin.Instance.Translation.CommandPlayerNotFound;
                return false;
            }
            
            if (!Int32.TryParse(arguments.At(1), out int limit)) {
                response = Plugin.Instance.Translation.SeeRecordsCommandLimitNotANumber;
                return false;
            } if (limit > 10 || limit < 1) {
                limit = 10;
            }
            
            string entryTypes = string.Join(" ", arguments.Array, 3, arguments.Count - 2);
            List<EntryBase> entries = new List<EntryBase>();
            if (entryTypes.Contains("report")) {
                entries = entries.Concat(Plugin.Database.GetReportsByTargetId(target.UserId)).ToList();
            } if (entryTypes.Contains("warn")) {
                entries = entries.Concat(Plugin.Database.GetWarnsByTargetId(target.UserId)).ToList();
            } if (entryTypes.Contains("kick")) {
                entries = entries.Concat(Plugin.Database.GetKicksByTargetId(target.UserId)).ToList();
            } if (entryTypes.Contains("ban")) {
                entries = entries.Concat(Plugin.Database.GetIdBansByTargetId(target.UserId)).ToList();
            }

            if (entries.Count == 0) {
                response = Plugin.Instance.Translation.SeeRecordsCommandNoEntries;
                return false;
            }

            entries = entries.OrderBy(entry => entry.IssuedAt).Take(limit).ToList();
            
            SeeRecords.SendDiscordWebhook(entries);

            response = Plugin.Instance.Translation.SeeRecordsCommandSuccess.Replace("{entriesAmount}", entries.Count.ToString());
            return true;
        }
    }
}