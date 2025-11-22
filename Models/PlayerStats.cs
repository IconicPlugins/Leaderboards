using System;

namespace ICN.Leaderboards.Models
{
    public class PlayerStats
    {
        public string SteamId { get; set; }
        public string PlayerName { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public int Headshots { get; set; }
        public double Accuracy { get; set; }
        public long Playtime { get; set; }

        public double KDRatio => Deaths > 0 ? Math.Round((double)Kills / Deaths, 2) : Kills;
        
        public string FormattedPlaytime
        {
            get
            {
                var timeSpan = TimeSpan.FromSeconds(Playtime);
                if (timeSpan.TotalHours >= 1)
                    return $"{(int)timeSpan.TotalHours}h {timeSpan.Minutes}m";
                else if (timeSpan.TotalMinutes >= 1)
                    return $"{(int)timeSpan.TotalMinutes}m";
                else
                    return $"{(int)timeSpan.TotalSeconds}s";
            }
        }
    }
}
