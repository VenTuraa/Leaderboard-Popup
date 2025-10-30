using UnityEngine;

namespace Leaderboard
{
    public interface IPlayerItemView
    {
        void SetRank(int rank);
        void SetPlayerName(string name);
        void SetScore(int score);

        void SetNameColor(Color color);
        void SetScoreColor(Color color);

        void ShowAvatarLoading(bool isVisible);
        void ShowAvatar(bool isVisible);
        void SetAvatarSprite(Sprite sprite);

        void ShowTypeLoading(bool isVisible);
        void ShowTypeIcon(bool isVisible);
        void SetTypeIconSprite(Sprite sprite);
    }
}


