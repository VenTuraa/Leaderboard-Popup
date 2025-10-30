using System;

namespace Leaderboard
{
    [Serializable]
    public class PlayerData
    {
        public string Name;
        public int Score;
        public string Avatar;
        public PlayerType Type;
        public string TypeImageUrl;
    }

    [Serializable]
    public enum PlayerType
    {
        Default = 0,
        Bronze = 1,
        Silver = 2,
        Gold = 3,
        Diamond = 4
    }
}
