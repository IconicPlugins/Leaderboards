using ICN.Leaderboards.Models;
using Newtonsoft.Json;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ICN.Leaderboards.Discord
{
    public class DiscordWebhookSender
    {
        private readonly string _webhookUrl;
        private static readonly HttpClient _httpClient = new HttpClient();

        public DiscordWebhookSender(string webhookUrl)
        {
            _webhookUrl = webhookUrl;
        }

        public async Task SendLeaderboardAsync(List<PlayerStats> players, LeaderboardsConfiguration config)
        {
            if (string.IsNullOrEmpty(_webhookUrl) || _webhookUrl == "https://discord.com/api/webhooks/your_webhook_url")
            {
                Logger.LogWarning("Discord Webhook URL is not configured. Please set a valid webhook URL in the configuration.");
                return;
            }

            if (players == null || players.Count == 0)
            {
                Logger.LogWarning("No players to display in leaderboard.");
                return;
            }

            try
            {
                StringBuilder description = new StringBuilder();
                int rank = 1;
                
                foreach (var player in players)
                {
                    string medal = GetMedalEmoji(rank);
                    description.AppendLine($"**{medal} {EscapeMarkdown(player.PlayerName)}**");
                    
                    StringBuilder statsLine = new StringBuilder();
                    statsLine.Append($"⚔️ Kills: **{player.Kills}** | ☠️ Deaths: **{player.Deaths}**");
                    
                    if (config.ShowKDRatio)
                        statsLine.Append($" | 📊 K/D: **{player.KDRatio}**");
                    
                    statsLine.Append($" | 🎯 HS: **{player.Headshots}**");
                    
                    if (config.ShowAccuracy)
                        statsLine.Append($" | 🎯 Acc: **{player.Accuracy:F1}%**");
                    
                    if (config.ShowPlaytime)
                        statsLine.Append($" | ⏱️ Time: **{player.FormattedPlaytime}**");
                    
                    description.AppendLine(statsLine.ToString());
                    
                    if (rank < players.Count)
                        description.AppendLine();
                    
                    rank++;
                }

                int embedColor = ParseHexColor(config.EmbedColor);

                var payload = new
                {
                    embeds = new[]
                    {
                        new
                        {
                            title = "🏆 Top Players Leaderboard 🏆",
                            description = description.ToString(),
                            color = embedColor,
                            footer = new { text = $"Sorted by {config.LeaderboardSortBy} • Unturned Leaderboards" },
                            timestamp = DateTime.UtcNow.ToString("o")
                        }
                    }
                };

                // Serialize with proper UTF-8 encoding for emoji support
                string jsonPayload = JsonConvert.SerializeObject(payload, new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.Default
                });
                
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_webhookUrl, content);
                
                if (response.IsSuccessStatusCode)
                {
                    Logger.Log($"Leaderboard sent to Discord successfully. ({players.Count} players)");
                }
                else
                {
                    Logger.LogWarning($"Discord webhook returned status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Logger.LogException(ex, "Network error sending leaderboard to Discord.");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Error sending leaderboard to Discord.");
            }
        }

        private string GetMedalEmoji(int rank)
        {
            return rank switch
            {
                1 => "🥇",
                2 => "🥈",
                3 => "🥉",
                _ => $"**#{rank}**"
            };
        }

        private string EscapeMarkdown(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            
            return text.Replace("*", "\\*")
                       .Replace("_", "\\_")
                       .Replace("`", "\\`")
                       .Replace("~", "\\~");
        }

        private int ParseHexColor(string hexColor)
        {
            try
            {
                if (string.IsNullOrEmpty(hexColor))
                    return 16761035; // Default gold

                hexColor = hexColor.TrimStart('#');
                return Convert.ToInt32(hexColor, 16);
            }
            catch
            {
                Logger.LogWarning($"Invalid hex color '{hexColor}', using default gold color.");
                return 16761035; // Default gold
            }
        }
    }
}
