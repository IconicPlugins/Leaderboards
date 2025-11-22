using ICN.Leaderboards.Models;
using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ICN.Leaderboards.Database
{
    public class MySQLDatabaseProvider
    {
        private readonly string _connectionString;

        public MySQLDatabaseProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<PlayerStats>> GetTopPlayersAsync(int count, string sortBy = "Kills")
        {
            List<PlayerStats> players = new List<PlayerStats>();

            if (count <= 0)
            {
                Logger.LogWarning("Invalid leaderboard count requested. Must be greater than 0.");
                return players;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    
                    // Validate and sanitize sort column
                    string orderByColumn = GetValidSortColumn(sortBy);
                    string query = $"SELECT SteamId, Name, Kills, PVPDeaths, Headshots, Zombies, Playtime FROM PlayerStats ORDER BY {orderByColumn} DESC LIMIT @Count";
                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Count", count);
                        
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                try
                                {
                                    int kills = reader["Kills"] != DBNull.Value ? Convert.ToInt32(reader["Kills"]) : 0;
                                    int deaths = reader["PVPDeaths"] != DBNull.Value ? Convert.ToInt32(reader["PVPDeaths"]) : 0;
                                    int headshots = reader["Headshots"] != DBNull.Value ? Convert.ToInt32(reader["Headshots"]) : 0;
                                    int zombies = reader["Zombies"] != DBNull.Value ? Convert.ToInt32(reader["Zombies"]) : 0;
                                    
                                    // Calculate accuracy as headshots/kills percentage
                                    double accuracy = kills > 0 ? (headshots / (double)kills) * 100.0 : 0.0;
                                    
                                    var stats = new PlayerStats
                                    {
                                        SteamId = reader["SteamId"]?.ToString() ?? "Unknown",
                                        PlayerName = reader["Name"]?.ToString() ?? "Unknown Player",
                                        Kills = kills,
                                        Deaths = deaths,
                                        Headshots = headshots,
                                        Zombies = zombies,
                                        Accuracy = accuracy,
                                        Playtime = reader["Playtime"] != DBNull.Value ? Convert.ToInt64(reader["Playtime"]) : 0
                                    };
                                    
                                    players.Add(stats);
                                }
                                catch (Exception ex)
                                {
                                    Logger.LogWarning($"Error parsing player stats row: {ex.Message}");
                                }
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Logger.LogException(ex, "MySQL error fetching top players from database.");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Error fetching top players from MySQL database.");
            }

            return players;
        }

        private string GetValidSortColumn(string sortBy)
        {
            switch (sortBy?.ToLower())
            {
                case "kills":
                    return "Kills";
                case "kdratio":
                    return "(CASE WHEN Deaths > 0 THEN Kills / Deaths ELSE Kills END)";
                case "headshots":
                    return "Headshots";
                case "accuracy":
                    return "Accuracy";
                case "playtime":
                    return "Playtime";
                default:
                    Logger.LogWarning($"Invalid sort column '{sortBy}', defaulting to Kills");
                    return "Kills";
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Failed to connect to MySQL database.");
                return false;
            }
        }
    }
}
