using System.ComponentModel;
using Exiled.API.Interfaces;

namespace SiteGuard {
    public class Config : IConfig {
        [Description("This is the configuration tab for the SiteGuard plugin. Below you can enable or disable the plugin.")] 
        public bool IsEnabled { get; set; } = true;
        [Description("Whether debug messages should be shown in the console.")]
        public bool Debug { get; set; } = false;
        [Description("Wheter to hyperlink to the users Steam account when their id is shown in a Discord message or embed.")]
        public bool HyperlinkToSteamAccount { get; set; } = true;
        
        [Description(
            "Here you can add Discord webhook URLs to integrate your SCP:SL server with your Discord server." 
            + "\nIf you don't want info about a certain event to be sent, simply leave that config option empty."
        )]
        public string ReportWebhookUrl { get; set; } = "";
        public string WarnWebhookUrl { get; set; } = "";
        public string KickWebhookUrl { get; set; } = "";
        public string BanWebhookUrl { get; set; } = "";
        public string SeeRecordsWebhookUrl { get; set; } = "";
        
        [Description("The color the embed for each module should have if webhooks are enabled.")]
        public string ReportEmbedColor { get; set; } = "#4444FF";
        public string IdBanEmbedColor { get; set; } = "#FF4444";
        public string IpBanEmbedColor { get; set; } = "#FF4444";
        public string KickEmbedColor { get; set; } = "#FF7744";
        public string WarnEmbedColor { get; set; } = "#FFFF77";
    }
}