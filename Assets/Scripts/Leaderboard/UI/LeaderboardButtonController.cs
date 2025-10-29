using UnityEngine;
using UnityEngine.UI;
using SimplePopupManager;

namespace Leaderboard
{
    public class LeaderboardButtonController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button leaderboardButton;
        [SerializeField] private string popupName = "LeaderboardPopup";

        private IPopupManagerService _popupManager;

        private void Start()
        {
            // Initialize popup manager
            _popupManager = new PopupManagerServiceService();

            // Setup button
            if (leaderboardButton != null)
            {
                leaderboardButton.onClick.RemoveAllListeners();
                leaderboardButton.onClick.AddListener(OpenLeaderboardPopup);
            }
        }

        private void OpenLeaderboardPopup()
        {
            if (_popupManager != null)
            {
                _popupManager.OpenPopup(popupName, _popupManager);
            }
            else
            {
                Debug.LogError("PopupManager not initialized");
            }
        }

        private void OnDestroy()
        {
            if (leaderboardButton != null)
            {
                leaderboardButton.onClick.RemoveAllListeners();
            }
        }
    }
}
