using ICN.Leaderboards.Database;
using ICN.Leaderboards.Discord;
using Rocket.Core.Plugins;
using System.Collections.Generic;
using ICN.Leaderboards.Models;
using System.Threading.Tasks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace ICN.Leaderboards
{
    public class LeaderboardsPlugin : RocketPlugin<LeaderboardsConfiguration>
    {
        public static LeaderboardsPlugin Instance { get; private set; }
        public MySQLDatabaseProvider DatabaseProvider { get; private set; }
        public DiscordWebhookSender WebhookSender { get; private set; }
        
        private float _autoPostTimer = 0f;

        protected override void Load()
        {
            Instance = this;
            Logger.Log("ICN.Leaderboards loading...");

            DatabaseProvider = new MySQLDatabaseProvider(Configuration.Instance.MySQLConnectionString);
            WebhookSender = new DiscordWebhookSender(Configuration.Instance.WebhookUrl);

            // Test database connection
            Task.Run(async () =>
            {
                bool connected = await DatabaseProvider.TestConnectionAsync();
                if (connected)
                    Logger.Log("Successfully connected to MySQL database.");
                else
                    Logger.LogWarning("Failed to connect to MySQL database. Check your connection string.");
            });

            Logger.Log("ICN.Leaderboards loaded!");
        }

        protected override void Unload()
        {
            Logger.Log("ICN.Leaderboards unloaded!");
            _autoPostTimer = 0f;
            Instance = null;
        }

        private void FixedUpdate()
        {
            if (Configuration.Instance.AutoPostIntervalMinutes > 0)
            {
                _autoPostTimer += Time.fixedDeltaTime;
                
                float intervalSeconds = Configuration.Instance.AutoPostIntervalMinutes * 60f;
                
                if (_autoPostTimer >= intervalSeconds)
                {
                    _autoPostTimer = 0f;
                    Task.Run(async () => await PostLeaderboardAsync());
                }
            }
        }

        public async Task PostLeaderboardAsync()
        {
            try
            {
                List<PlayerStats> topPlayers = await DatabaseProvider.GetTopPlayersAsync(
                    Configuration.Instance.LeaderboardCount,
                    Configuration.Instance.LeaderboardSortBy
                );
                
                await WebhookSender.SendLeaderboardAsync(topPlayers, Configuration.Instance);
            }
            catch (System.Exception ex)
            {
                Logger.LogException(ex, "Error posting leaderboard.");
            }
        }
    }
}
