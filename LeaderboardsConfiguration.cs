using Rocket.API;

namespace ICN.Leaderboards
{
    public class LeaderboardsConfiguration : IRocketPluginConfiguration
    {
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
            AutoPostIntervalMinutes = 0;
            EmbedColor = "#FFD700"; // Gold
        }
    }
}
