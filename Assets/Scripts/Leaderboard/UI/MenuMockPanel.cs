using UnityEngine;
using UnityEngine.UI;
using SimplePopupManager;
using TMPro;
using Zenject;

namespace Leaderboard
{
    public class MenuMockPanel : MonoBehaviour
    {
        const string LEADERBOARD = "Leaderboard";
        const string LOADING = "Loading";

        [Header("UI References")] [SerializeField]
        private Button btnLeaderboard;

        [SerializeField] private TMP_Text txtButton;
        [SerializeField] private string popupName = "LeaderboardPopup";

        private IPopupManagerService _popupManager;

        [Inject]
        public void Construct(IPopupManagerService popupManager)
        {
            _popupManager = popupManager;
        }

        private void Start()
        {
            btnLeaderboard?.onClick.AddListener(OpenLeaderboardPopup);
        }

        private void OpenLeaderboardPopup()
        {
            SetLoadingState(true);
            if (_popupManager != null)
            {
                var args = new LeaderboardPopupParams
                {
                    PopupManager = _popupManager,
                    OnOpened = () =>
                    {
                         btnLeaderboard.gameObject.SetActive(false);
                    },
                    OnClosed = () =>
                    {
                        btnLeaderboard.gameObject.SetActive(true);
                        SetLoadingState(false);
                    }
                };
                _popupManager.OpenPopup(popupName, args);
            }
            else
            {
                Debug.LogError("PopupManager not initialized");
            }
        }

        private void SetLoadingState(bool isLoading)
        {
            btnLeaderboard.interactable = !isLoading;
            txtButton.SetText(isLoading ? LOADING : LEADERBOARD);
        }

        private void OnDestroy()
        {
            if (btnLeaderboard)
            {
                btnLeaderboard.onClick.RemoveAllListeners();
            }
        }
    }
}