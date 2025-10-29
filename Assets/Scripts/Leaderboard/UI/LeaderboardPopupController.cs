using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using SimplePopupManager;

namespace Leaderboard
{
    public class LeaderboardPopupController : MonoBehaviour, IPopupInitialization
    {
        [Header("UI References")]
        [SerializeField] private Button closeButton;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Transform contentParent;
        [SerializeField] private GameObject playerItemPrefab;

        [Header("Popup Settings")]
        [SerializeField] private string popupName = "LeaderboardPopup";

        private IPopupManagerService _popupManager;
        private List<PlayerItemController> _playerItems = new List<PlayerItemController>();

        public async Task Init(object param)
        {
            // Get popup manager reference
            _popupManager = param as IPopupManagerService;
            if (_popupManager == null)
            {
                Debug.LogError("PopupManager not provided to LeaderboardPopupController");
                return;
            }

            // Setup close button
            if (closeButton != null)
            {
                closeButton.onClick.RemoveAllListeners();
                closeButton.onClick.AddListener(ClosePopup);
            }

            // Load and display leaderboard data
            await LoadLeaderboardData();
        }

        private async Task LoadLeaderboardData()
        {
            // Load leaderboard data from JSON
            var leaderboardData = LoadLeaderboardFromJson();
            
            if (leaderboardData?.leaderboard != null)
            {
                await DisplayLeaderboard(leaderboardData.leaderboard);
            }
            else
            {
                Debug.LogError("Failed to load leaderboard data");
            }
        }

        private LeaderboardData LoadLeaderboardFromJson()
        {
            var jsonText = Resources.Load<TextAsset>("Leaderboard");
            if (jsonText != null)
            {
                return JsonUtility.FromJson<LeaderboardData>(jsonText.text);
            }
            return null;
        }

        private async Task DisplayLeaderboard(PlayerData[] players)
        {
            // Clear existing items
            foreach (var item in _playerItems)
            {
                if (item != null)
                    Destroy(item.gameObject);
            }
            _playerItems.Clear();

            // Create player items
            for (int i = 0; i < players.Length; i++)
            {
                var playerItem = Instantiate(playerItemPrefab, contentParent);
                var controller = playerItem.GetComponent<PlayerItemController>();
                
                if (controller != null)
                {
                    await controller.Initialize(players[i], i + 1);
                    _playerItems.Add(controller);
                }
            }
        }

        private void ClosePopup()
        {
            if (_popupManager != null)
            {
                _popupManager.ClosePopup(popupName);
            }
        }
    }
}
