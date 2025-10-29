using System;

namespace Leaderboard
{
    [Serializable]
    public class PlayerData
    {
        public string name;
        public int score;
        public string avatar;
        public PlayerType type;
        public string typeImageUrl;
    }

    [Serializable]
    public enum PlayerType
    {
        Diamond,
        Gold,
        Silver,
        Bronze,
        Default
    }

    [Serializable]
    public class LeaderboardData
    {
        public PlayerData[] leaderboard;
    }
}
