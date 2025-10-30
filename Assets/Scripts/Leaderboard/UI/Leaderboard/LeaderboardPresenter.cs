using System.Threading.Tasks;
using UnityEngine;
using SimplePopupManager;

namespace Leaderboard
{
    public class LeaderboardPresenter
    {
        private readonly ILeaderboardView _view;
        private readonly IPopupManagerService _popupManager;
        private readonly string _popupAddress;
        private readonly System.Action _onOpened;
        private readonly System.Action _onClosed;

        public LeaderboardPresenter(
            ILeaderboardView view,
            IPopupManagerService popupManager,
            string popupAddress,
            System.Action onOpened,
            System.Action onClosed)
        {
            _view = view;
            _popupManager = popupManager;
            _popupAddress = popupAddress;
            _onOpened = onOpened;
            _onClosed = onClosed;
        }

        public async Task Initialize()
        {
            var players = LoadPlayersFromJson();
            if (players == null)
            {
                Debug.LogError("Failed to load leaderboard data");
                return;
            }

            await DisplayLeaderboard(players);

            _onOpened?.Invoke();
        }

        public void HandleCloseRequested()
        {
            if (_popupManager != null && !string.IsNullOrEmpty(_popupAddress))
            {
                _popupManager.ClosePopup(_popupAddress);
            }
            _onClosed?.Invoke();
        }

        [System.Serializable]
        private class PlayerDataDto
        {
            public string name;
            public int score;
            public string avatar;
            public string type; 
            public string typeImageUrl;
        }

        [System.Serializable]
        private class LeaderboardDataDto
        {
            public PlayerDataDto[] leaderboard;
        }

        private PlayerData[] LoadPlayersFromJson()
        {
            var jsonText = Resources.Load<TextAsset>("Leaderboard");
            if (!jsonText)
                return null;

            var dto = JsonUtility.FromJson<LeaderboardDataDto>(jsonText.text);
            if (dto?.leaderboard == null)
                return null;

            var players = new PlayerData[dto.leaderboard.Length];
            for (int i = 0; i < dto.leaderboard.Length; i++)
            {
                var d = dto.leaderboard[i];
                players[i] = new PlayerData
                {
                    Name = d.name,
                    Score = d.score,
                    Avatar = d.avatar,
                    Type = ParsePlayerType(d.type),
                    TypeImageUrl = d.typeImageUrl
                };
            }
            return players;
        }

        private PlayerType ParsePlayerType(string s)
        {
            if (string.IsNullOrEmpty(s))
                return PlayerType.Default;

            if (System.Enum.TryParse(s, true, out PlayerType value))
                return value;

            return PlayerType.Default;
        }

        private async Task DisplayLeaderboard(PlayerData[] players)
        {
            _view.ClearItems();

            for (var i = 0; i < players.Length; i++)
            {
                PlayerItemView item = _view.CreatePlayerItem();
                if (item != null)
                {
                    await _view.InitializePlayerItem(item, players[i], i + 1);
                }
            }
        }
    }
}


