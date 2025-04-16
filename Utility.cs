using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Exiled.API.Features;

namespace SiteGuard {
    public static class Utility {
        private static readonly Regex _sizeTagRegex = new Regex("<size=.*?<\\/size>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex _colorTagRegex = new Regex("<color=.*?<\\/color>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex _hexCharacterRegex = new Regex ("^[0-9A-F]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex _steamIdRegex = new Regex ("^\\d{17}@steam$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Gets the name of the server.
        /// This method exists because <see cref="Exiled.API.Features.Server.Name"/> returns
        /// the entire name with size and color tags, and the Exiled version at the end.
        /// </summary>
        /// <returns>The name of the server like it is shown in the player list.</returns>
        public static string FormatServerName() {
            string serverName = Server.Name;
            serverName = _sizeTagRegex.Replace(serverName, "").Trim();
            serverName = _colorTagRegex.Replace(serverName, "").Trim();
            return serverName;
        }
        
        /// <summary>
        /// Takes a hex color string and parses it into a hex integer.
        /// </summary>
        /// <param name="hexColor">
        /// Hex color string to parse
        /// </param>
        /// <remarks>
        /// Examples for valid formats for the hex color string are:<para />
        /// #ff44EE, #F4e, FF44EE, f4e
        /// </remarks>
        /// <returns>The parsed hex color integer</returns>
        public static int ParseIntHexColor(string hexColor) {
            if (hexColor == null) {
                throw new ArgumentNullException(nameof(hexColor), "Hex color string cannot be null");
            }

            if (hexColor.StartsWith("#")) {
                hexColor = hexColor.Substring(1);
            }

            if (hexColor.Length == 3) {
                hexColor = new string(new char[] { hexColor[0], hexColor[0], hexColor[1], hexColor[1], hexColor[2], hexColor[2] });
            }

            if (hexColor.Length == 6 && _hexCharacterRegex.IsMatch(hexColor)) {
                return int.Parse(hexColor, NumberStyles.HexNumber);
            } else {
                throw new FormatException("Invalid hex color string format.");
            }
        }

        
        /// <summary>
        /// Takes a Unix timestamp in seconds and formats it into
        /// a human-readable duration in minutes, hours, days or years.
        /// </summary>
        /// <param name="seconds">Unix timestamp in seconds</param>
        public static string FormatUnixTimeToDuration(double seconds) {
            double minutes = seconds / 60d;
            if (minutes < 60d) {
                return $"{minutes:0.##} {(minutes == 1d ? "minute" : "minutes")}";
            }
            
            double hours = minutes / 60d;
            if (hours < 24d) {
                return $"{hours:0.##} {(hours == 1d ? "hour" : "hours")}";
            }
            
            double days = hours / 24d;
            if (days < 365d) {
                return $"{days:0.##} {(days == 1d ? "day" : "days")}";
            }
            
            double years = days / 365d;
            return $"{years:0.##} {(years == 1d ? "year" : "years")}";
        }
        
        /// <param name="userId">UserId in the [SteamCommunityId]@steam format</param>
        /// <returns>
        /// Depending on config, either the UserId with a Markdown hyperlink to the users Steam account
        /// or just the UserId in a Markdown code block.
        /// </returns>
        public static string FormatUserIdForDiscord(string userId) {
            if (string.IsNullOrWhiteSpace(userId) || !userId.Contains("@steam")) {
                return "`Invalid UserId`";
            }
            
            if (Plugin.Instance.Config.HyperlinkToSteamAccount) {
                return $"[{userId}](https://steamcommunity.com/profiles/{userId.Split('@')[0]}/)";
            } else {
                return $"`{userId}`";
            }
        }
        
        /// <returns>True if the SteamId is in the [SteamCommunityId]@steam format and has 17 digits.</returns>
        public static bool IsSteamIdValidFormat(string steamId) {
            return _steamIdRegex.IsMatch(steamId);
        }
    }
}