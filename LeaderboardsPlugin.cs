using ICN.Leaderboards.Database;
using ICN.Leaderboards.Discord;
using Rocket.Core.Plugins;
using System.Collections.Generic;
using ICN.Leaderboards.Models;
using System.Threading.Tasks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;
using SDG.Unturned;

namespace ICN.Leaderboards
{
    public class LeaderboardsPlugin : RocketPlugin<LeaderboardsConfiguration>
    {
        public static LeaderboardsPlugin Instance { get; private set; }
        public MySQLDatabaseProvider DatabaseProvider { get; private set; }
        public DiscordWebhookSender WebhookSender { get; private set; }
        
        private float _autoPostTimer = 0f;
        private float _startupPostTimer = 0f;
        private bool _hasPostedOnStartup = false;

        protected override void Load()
        {
            Instance = this;
            
            // Cool loading banner
            Logger.Log("╔═══════════════════════════════════════╗");
            Logger.Log("║   Leaderboards by Iconic Plugins      ║");
            Logger.Log("╚═══════════════════════════════════════╝");
            Logger.Log($"Licensed to: {Provider.serverName}");
            Logger.Log("Copyright © 2025 Iconic Plugins. All Rights Reserved.");

            try
            {
                DatabaseProvider = new MySQLDatabaseProvider(Configuration.Instance.MySQLConnectionString);
                WebhookSender = new DiscordWebhookSender(Configuration.Instance.WebhookUrl);

                Logger.Log("ICN.Leaderboards loaded successfully!");
                Logger.Log($"Leaderboard count: {Configuration.Instance.LeaderboardCount}");
                Logger.Log($"Sort by: {Configuration.Instance.LeaderboardSortBy}");
                Logger.Log($"Auto-post interval: {Configuration.Instance.AutoPostIntervalMinutes} minutes");
                Logger.Log("Leaderboard will be posted to Discord in 10 seconds...");
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Failed to load ICN.Leaderboards: {ex.Message}");
            }
        }

        protected override void Unload()
        {
            Logger.Log("ICN.Leaderboards unloaded successfully!");
            _autoPostTimer = 0f;
            Instance = null;
        }

        private void FixedUpdate()
        {
            // Post leaderboard 10 seconds after startup
            if (!_hasPostedOnStartup)
            {
                _startupPostTimer += Time.fixedDeltaTime;
                
                if (_startupPostTimer >= 10f) // 10 seconds after load
                {
                    _hasPostedOnStartup = true;
                    Logger.Log("Posting startup leaderboard to Discord...");
                    PostLeaderboard();
                }
            }
            
            // Regular auto-posting
            if (Configuration.Instance.AutoPostIntervalMinutes > 0)
            {
                _autoPostTimer += Time.fixedDeltaTime;
                
                float intervalSeconds = Configuration.Instance.AutoPostIntervalMinutes * 60f;
                
                if (_autoPostTimer >= intervalSeconds)
                {
                    _autoPostTimer = 0f;
                    PostLeaderboard();
                }
            }
        }

        public async void PostLeaderboard()
        {
            try
            {
                List<PlayerStats> topPlayers = await DatabaseProvider.GetTopPlayersAsync(
                    Configuration.Instance.LeaderboardCount,
                    Configuration.Instance.LeaderboardSortBy
                );
                
                WebhookSender.SendLeaderboard(
                    topPlayers,
                    Configuration.Instance.LeaderboardSortBy,
                    Configuration.Instance.ShowKDRatio,
                    Configuration.Instance.ShowAccuracy,
                    Configuration.Instance.ShowPlaytime,
                    Configuration.Instance.EmbedColor,
                    ref Configuration.Instance.LastMessageId
                );
                
                // Save configuration to persist the message ID
                Configuration.Save();
            }
            catch (System.Exception ex)
            {
                Logger.LogError($"Error posting leaderboard: {ex.Message}");
            }
        }
    }
}
