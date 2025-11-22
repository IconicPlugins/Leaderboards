using Rocket.API;
using Rocket.Unturned.Chat;
using System.Collections.Generic;
using UnityEngine;

namespace ICN.Leaderboards.Commands
{
    public class CommandLeaderboard : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "postleaderboard";

        public string Help => "Posts the leaderboard to Discord.";

        public string Syntax => "";

        public List<string> Aliases => new List<string> { "leaderboard", "lb" };

        public List<string> Permissions => new List<string> { "leaderboards.post" };

        private static float _lastUsed = 0f;
        private const float COOLDOWN = 5f;

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (Time.realtimeSinceStartup - _lastUsed < COOLDOWN)
            {
                UnturnedChat.Say(caller, "Please wait before posting the leaderboard again.", Color.yellow);
                return;
            }

            _lastUsed = Time.realtimeSinceStartup;

            UnturnedChat.Say(caller, "Posting leaderboard to Discord...", Color.cyan);
            
            try
            {
                LeaderboardsPlugin.Instance.PostLeaderboard();
                UnturnedChat.Say(caller, "Leaderboard posted to Discord successfully!", Color.green);
            }
            catch (System.Exception ex)
            {
                UnturnedChat.Say(caller, "Failed to post leaderboard. Check console for details.", Color.red);
                Rocket.Core.Logging.Logger.LogError($"Error in /postleaderboard command: {ex.Message}");
            }
        }
    }
}
