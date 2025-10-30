using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using SimplePopupManager;

namespace Leaderboard
{
    public class LeaderboardPopupView : MonoBehaviour, IPopupInitialization, ILeaderboardView
    {
        [Header("UI References")]
        [SerializeField] private Button closeButton;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Transform contentParent;
        [SerializeField] private GameObject playerItemPrefab;

        [Header("Popup Settings")]
        [SerializeField] private string popupName = "LeaderboardPopup";

        private IPopupManagerService _popupManager;
        private List<PlayerItemView> _playerItems = new ();
        private LeaderboardPresenter _presenter;

        public async Task Init(object param)
        {
            if (param is LeaderboardPopupParams p)
            {
                _popupManager = p.PopupManager;
                _presenter = new LeaderboardPresenter(this, _popupManager, popupName, p.OnOpened, p.OnClosed);
            }
            else
            {
                _popupManager = param as IPopupManagerService;
                _presenter = new LeaderboardPresenter(this, _popupManager, popupName, null, null);
            }
            if (_popupManager == null)
            {
                Debug.LogError("PopupManager not provided to LeaderboardPopupController");
                return;
            }

            if (closeButton)
            {
                closeButton.onClick.RemoveAllListeners();
                closeButton.onClick.AddListener(() => _presenter.HandleCloseRequested());
            }

            await _presenter.Initialize();
        }

        public void ClearItems()
        {
            foreach (PlayerItemView item in _playerItems.Where(item => item))
            {
                Destroy(item.gameObject);
            }

            _playerItems.Clear();
        }

        public PlayerItemView CreatePlayerItem()
        {
            var go = Instantiate(playerItemPrefab, contentParent);
            var controller = go.GetComponent<PlayerItemView>();
            if (controller)
            {
                _playerItems.Add(controller);
            }
            return controller;
        }

        public Task InitializePlayerItem(PlayerItemView view, PlayerData data, int rank)
        {
            return view.Initialize(data, rank);
        }

        void ILeaderboardView.ClosePopup()
        {
            _presenter?.HandleCloseRequested();
        }
    }
}
