using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Leaderboard
{
    public class PlayerItemView : MonoBehaviour, IPlayerItemView
    {
        [Header("UI References")]
        [SerializeField] private  TMP_Text rankText;
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Image avatarImage;
        [SerializeField] private TMP_Text loadingText;
        [SerializeField] private Image playerTypeIcon;
        [SerializeField] private TMP_Text typeLoadingText;
        [SerializeField] private Image backgroundImage;

		private PlayerItemPresenter _presenter;

		public Task Initialize(PlayerData playerData, int rank)
		{
			_presenter = new PlayerItemPresenter(this);
			return _presenter.Initialize(playerData, rank);
		}

		public void SetRank(int rank)
		{
			if (rankText != null)
				rankText.text = rank.ToString();
		}

		public void SetPlayerName(string name)
		{
			if (playerNameText)
				playerNameText.text = name;
		}

		public void SetScore(int score)
		{
			if (scoreText)
				scoreText.text = score.ToString();
		}
		
		public void SetNameColor(Color color)
		{
			if (playerNameText)
				playerNameText.color = color;
		}

		public void SetScoreColor(Color color)
		{
			if (scoreText)
				scoreText.color = color;
		}
		
		public void ShowAvatarLoading(bool isVisible)
		{
			if (loadingText)
				loadingText.gameObject.SetActive(isVisible);
		}

		public void ShowAvatar(bool isVisible)
		{
			if (avatarImage)
				avatarImage.gameObject.SetActive(isVisible);
		}

		public void SetAvatarSprite(Sprite sprite)
		{
			if (avatarImage)
				avatarImage.sprite = sprite;
		}

		public void ShowTypeLoading(bool isVisible)
		{
			if (typeLoadingText)
				typeLoadingText.gameObject.SetActive(isVisible);
		}

		public void ShowTypeIcon(bool isVisible)
		{
			if (playerTypeIcon)
				playerTypeIcon.gameObject.SetActive(isVisible);
		}

		public void SetTypeIconSprite(Sprite sprite)
		{
			if (playerTypeIcon)
				playerTypeIcon.sprite = sprite;
		}
    }
}
