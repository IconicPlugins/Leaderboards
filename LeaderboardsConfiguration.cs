using Rocket.API;

namespace ICN.Leaderboards
{
    public class LeaderboardsConfiguration : IRocketPluginConfiguration
    {
        // Configuration version for migration tracking
        public int ConfigVersion = 1; // v1: Initial version
        
        public string WebhookUrl;
        public string MySQLConnectionString;
        public int LeaderboardCount;
        public bool IgnoreAdmins;
        public string LeaderboardSortBy; // Options: "Kills", "KDRatio", "Headshots", "Accuracy"
        public bool ShowKDRatio;
        public bool ShowAccuracy;
        public bool ShowPlaytime;
        public int AutoPostIntervalMinutes; // 0 = disabled
        public string EmbedColor; // Hex color code
        public string LastMessageId; // For editing instead of creating new messages

        public void LoadDefaults()
        {
            WebhookUrl = "https://discord.com/api/webhooks/your_webhook_url";
            MySQLConnectionString = "Server=127.0.0.1;Port=3306;Database=unturned;Uid=root;Pwd=password;";
            LeaderboardCount = 10;
            IgnoreAdmins = false;
            LeaderboardSortBy = "Kills";
            ShowKDRatio = true;
            ShowAccuracy = true;
            ShowPlaytime = false;
            AutoPostIntervalMinutes = 30; // Auto-post every 30 minutes
            EmbedColor = "#FFD700"; // Gold
            LastMessageId = ""; // Will be set after first post
        }
    }
}
