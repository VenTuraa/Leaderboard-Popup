using System.Threading.Tasks;

namespace Leaderboard
{
    public interface ILeaderboardView
    {
        void ClearItems();
        PlayerItemView CreatePlayerItem();
        Task InitializePlayerItem(PlayerItemView view, PlayerData data, int rank);
        void ClosePopup();
    }
}


