using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ICN.Leaderboards.Models;

namespace ICN.Leaderboards.Discord
{
    public class DiscordWebhookSender
    {
        private readonly string _webhookUrl;

        public DiscordWebhookSender(string webhookUrl)
        {
            _webhookUrl = webhookUrl;
        }

        public void SendLeaderboard(List<PlayerStats> players, string sortBy, bool showKD, bool showAccuracy, bool showPlaytime, string embedColor, ref string lastMessageId)
        {
            try
            {
                if (string.IsNullOrEmpty(_webhookUrl))
                {
                    Rocket.Core.Logging.Logger.LogWarning("Discord webhook URL is not configured.");
                    return;
                }

                if (players == null || players.Count == 0)
                {
                    Rocket.Core.Logging.Logger.LogWarning("No players to display in leaderboard.");
                    return;
                }

                // Parse embed color
                int color = ParseHexColor(embedColor);

                // Build rich description with better formatting for PVP
                var description = new StringBuilder();
                
                for (int i = 0; i < players.Count; i++)
                {
                    var player = players[i];
                    string medal = i == 0 ? "🥇" : i == 1 ? "🥈" : i == 2 ? "🥉" : $"**{i + 1}.**";
                    
                    // Escape markdown special characters
                    string safeName = EscapeMarkdown(player.PlayerName);
                    
                    // Player name with medal
                    description.AppendLine($"{medal} **{safeName}**");
                    
                    // First row: PVP stats
                    description.Append($"⚔️ Kills: **{player.Kills}** | ☠️ Deaths: **{player.Deaths}**");
                    
                    if (showKD)
                        description.Append($" | 📊 K/D: **{player.KDRatio:F2}**");
                    
                    description.AppendLine(); // New line after first row
                    
                    // Second row: Headshots, accuracy, and zombie kills
                    description.Append($"🎯 HS: **{player.Headshots}**");
                    
                    if (showAccuracy)
                        description.Append($" | 🎯 Acc: **{player.Accuracy:F1}%**");
                    
                    // Always show zombie kills for PVP servers
                    description.Append($" | 🧟 Zombies: **{player.Zombies}**");
                    
                    if (showPlaytime)
                        description.Append($" | ⏱️ Time: **{player.FormattedPlaytime}**");
                    
                    description.AppendLine();
                    
                    // Add spacing between players (except last one)
                    if (i < players.Count - 1)
                        description.AppendLine();
                }

                var embed = new
                {
                    title = "🏆 Top Players Leaderboard 🏆",
                    description = description.ToString(),
                    color = color,
                    footer = new
                    {
                        text = $"Sorted by {sortBy} • Unturned Leaderboards"
                    },
                    timestamp = DateTime.UtcNow.ToString("o")
                };

                var payload = new
                {
                    embeds = new[] { embed }
                };

                // Serialize with proper encoding
                string json = JsonConvert.SerializeObject(payload, new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.Default
                });

                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    
                    // Check if we should edit existing message or create new one
                    if (!string.IsNullOrEmpty(lastMessageId))
                    {
                        try
                        {
                            // Edit existing message using PATCH
                            string editUrl = $"{_webhookUrl}/messages/{lastMessageId}";
                            client.UploadString(editUrl, "PATCH", json);
                            Rocket.Core.Logging.Logger.Log($"Leaderboard updated in Discord. ({players.Count} players) [Edited Message ID: {lastMessageId}]");
                            return;
                        }
                        catch (Exception editEx)
                        {
                            Rocket.Core.Logging.Logger.LogWarning($"Failed to edit message (ID: {lastMessageId}), creating new one: {editEx.Message}");
                            lastMessageId = ""; // Reset so we create a new message
                        }
                    }
                    
                    // Create new message with wait=true to get message ID
                    string webhookUrlWithWait = _webhookUrl.Contains("?") ? $"{_webhookUrl}&wait=true" : $"{_webhookUrl}?wait=true";
                    string response = client.UploadString(webhookUrlWithWait, json);
                    
                    // Parse response to get message ID using JObject
                    try
                    {
                        var responseObj = Newtonsoft.Json.Linq.JObject.Parse(response);
                        string messageId = responseObj["id"]?.ToString();
                        
                        if (!string.IsNullOrEmpty(messageId))
                        {
                            lastMessageId = messageId; // Store for future edits
                            Rocket.Core.Logging.Logger.Log($"Leaderboard sent to Discord successfully. ({players.Count} players) [Message ID: {messageId}]");
                        }
                        else
                        {
                            Rocket.Core.Logging.Logger.Log($"Leaderboard sent to Discord successfully. ({players.Count} players)");
                        }
                    }
                    catch
                    {
                        Rocket.Core.Logging.Logger.Log($"Leaderboard sent to Discord successfully. ({players.Count} players)");
                    }
                }
            }
            catch (Exception ex)
            {
                Rocket.Core.Logging.Logger.LogError($"Failed to send Discord webhook: {ex.Message}");
            }
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
                Rocket.Core.Logging.Logger.LogWarning($"Invalid hex color '{hexColor}', using default gold color.");
                return 16761035; // Default gold
            }
        }
    }
}
