using System.ComponentModel;
using Exiled.API.Interfaces;

namespace SiteGuard {
    public class Translation : ITranslation {
        [Description("Command arguments")]
        public string CommandArgumentOnlinePlayer { get; set; } = "PlayerId/Name";
        public string CommandArgumentOfflinePlayer { get; set; } = "SteamId@steam";
        public string CommandArgumentReason { get; set; } = "Reason";
        public string CommandArgumentSeeRecordsLimit { get; set; } = "Limit";
        public string CommandArgumentSeeRecordsTypes { get; set; } = "Entry Types";
        
        [Description("Generic command responses")]
        public string CommandMissingPermissions { get; set; } = "You don't have permission to use this command.";
        public string CommandNotEnoughArguments { get; set; } = "Not enough arguments provided. Please look at the command usage.";
        public string CommandPlayerNotFound { get; set; } = "Player was not found.";
        public string CommandWrongSteamIdFormat { get; set; } = "SteamId is in a wrong format. A SteamId consists of 17 digits and ends on @steam";
        
        [Description("Warn command description and responses")]
        public string WarnCommandDescription { get; set; } = "Warns a player.";
        public string WarnCommandSuccess { get; set; } = "Player was warned.";
        
        [Description("Offline warn command description and responses")]
        public string OfflineWarnCommandDescription { get; set; } = "Warns a player who is offline.";   
        
        [Description("See records command description and responses")]
        public string SeeRecordsCommandDescription { get; set; } = "Sends reports, warns, kicks, and bans from a player to your Discord.";
        public string SeeRecordsCommandSuccess { get; set; } = "Sent {entriesAmount} entries for provided player to your Discord.";
        public string SeeRecordsCommandNoEntries { get; set; } = "No entries found for the provided player.";
        public string SeeRecordsCommandLimitNotANumber { get; set; } = "Limit must be a number.";
        public string SeeRecordsCommandNoWebhook { get; set; } = "No Discord webhook configured, nowhere to send the entries to.";
        
        [Description("Offline see records command description and responses")]
        public string OfflineSeeRecordsCommandDescription { get; set; } = "Sends reports, warns, kicks, and bans from a player who is offline to your Discord.";
        
        [Description("Labels and titles used in the report embed message sent to your Discord webhook")]
        public string ReportWebhookTitle { get; set; } = "Report";
        public string ReportWebhookServerName { get; set; } = "Server Name";
        public string ReportWebhookServerEndpoint { get; set; } = "Server Endpoint";
        public string ReportWebhookReporterName { get; set; } = "Reporter Name";
        public string ReportWebhookReporterId { get; set; } = "Reporter UserId";
        public string ReportWebhookReportedName { get; set; } = "Reported Name";
        public string ReportWebhookReportedId { get; set; } = "Reported UserId";
        public string ReportWebhookReason { get; set; } = "Reason";
        
        [Description("Labels and titles used in the ban and offline ban embed message sent to your Discord webhook")]
        public string BanWebhookTitle { get; set; } = "Ban";
        public string BanWebhookIssuerName { get; set; } = "Issuer Name";
        public string BanWebhookIssuerId { get; set; } = "Issuer UserId";
        public string BanWebhookBannedName { get; set; } = "Banned Name";
        public string BanWebhookBannedId { get; set; } = "Banned UserId";
        public string BanWebhookBannedIp { get; set; } = "Banned IP";
        public string BanWebhookType { get; set; } = "Ban Type";
        public string BanWebhookLength { get; set; } = "Ban Length";
        public string BanWebhookReason { get; set; } = "Reason";
        
        [Description("Labels and titles used in the kick embed message sent to your Discord webhook")]
        public string KickWebhookTitle { get; set; } = "Kick";
        public string KickWebhookIssuerName { get; set; } = "Issuer Name";
        public string KickWebhookIssuerId { get; set; } = "Issuer UserId";
        public string KickWebhookKickedName { get; set; } = "Kicked Name";
        public string KickWebhookKickedId { get; set; } = "Kicked UserId";
        public string KickWebhookReason { get; set; } = "Reason";
        
        [Description("Labels and titles used in the warn embed message sent to your Discord webhook")]
        public string WarnWebhookTitle { get; set; } = "Warn";
        public string WarnWebhookIssuerName { get; set; } = "Issuer Name";
        public string WarnWebhookIssuerId { get; set; } = "Issuer UserId";
        public string WarnWebhookKickedName { get; set; } = "Warned Name";
        public string WarnWebhookKickedId { get; set; } = "Warned UserId";
        public string WarnWebhookReason { get; set; } = "Reason";
    }
}