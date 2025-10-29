using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Leaderboard
{
    public class PlayerItemController : MonoBehaviour
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

        public async Task Initialize(PlayerData playerData, int rank)
        {
            // Set rank
            if (rankText != null)
                rankText.text = rank.ToString();

            // Set player name
            if (playerNameText != null)
                playerNameText.text = playerData.name;

            // Set score
            if (scoreText != null)
                scoreText.text = playerData.score.ToString();

            // Apply player type styling
            ApplyPlayerTypeStyling(playerData.type);

            // Load avatar and type image concurrently
            var avatarTask = LoadPlayerAvatar(playerData.avatar);
            var typeImageTask = LoadPlayerTypeImage(playerData.typeImageUrl);
            
            await Task.WhenAll(avatarTask, typeImageTask);
        }

        private void ApplyPlayerTypeStyling(PlayerType playerType)
        {
            Color typeColor = GetPlayerTypeColor(playerType);
            float typeScale = GetPlayerTypeScale(playerType);

            // Apply color to background
            if (backgroundImage != null)
            {
                var color = typeColor;
                color.a = 0.1f; // Make background semi-transparent
                backgroundImage.color = color;
            }

            // Apply color to text elements
            if (playerNameText != null)
                playerNameText.color = typeColor;

            if (scoreText != null)
                scoreText.color = typeColor;

            // Apply scale
            transform.localScale = Vector3.one * typeScale;
        }

        private Color GetPlayerTypeColor(PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.Diamond:
                    return new Color(0f, 1f, 1f, 1f); // Cyan
                case PlayerType.Gold:
                    return new Color(1f, 0.843f, 0f, 1f); // Gold
                case PlayerType.Silver:
                    return new Color(0.753f, 0.753f, 0.753f, 1f); // Silver
                case PlayerType.Bronze:
                    return new Color(0.804f, 0.498f, 0.196f, 1f); // Bronze
                default:
                    return Color.white; // Default
            }
        }

        private float GetPlayerTypeScale(PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.Diamond:
                    return 1.2f;
                case PlayerType.Gold:
                    return 1.1f;
                case PlayerType.Silver:
                    return 1.05f;
                case PlayerType.Bronze:
                case PlayerType.Default:
                default:
                    return 1.0f;
            }
        }

        private async Task LoadPlayerAvatar(string avatarUrl)
        {
            // Show loading text
            if (loadingText != null)
                loadingText.gameObject.SetActive(true);

            if (avatarImage != null)
                avatarImage.gameObject.SetActive(false);

            try
            {
                // Load avatar from service
                var texture = await AvatarService.Instance.LoadAvatar(avatarUrl);

                if (texture != null)
                {
                    // Create sprite from texture
                    var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    
                    if (avatarImage != null)
                    {
                        avatarImage.sprite = sprite;
                        avatarImage.gameObject.SetActive(true);
                    }
                }
                else
                {
                    // Show default avatar or placeholder
                    if (avatarImage != null)
                    {
                        avatarImage.gameObject.SetActive(true);
                        // You could set a default avatar sprite here
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error loading avatar: {e.Message}");
                if (avatarImage != null)
                    avatarImage.gameObject.SetActive(true);
            }
            finally
            {
                // Hide loading text
                if (loadingText != null)
                    loadingText.gameObject.SetActive(false);
            }
        }

        private async Task LoadPlayerTypeImage(string typeImageUrl)
        {
            // Show loading text for type image
            if (typeLoadingText != null)
                typeLoadingText.gameObject.SetActive(true);

            if (playerTypeIcon != null)
                playerTypeIcon.gameObject.SetActive(false);

            try
            {
                // Load type image from service
                var texture = await AvatarService.Instance.LoadAvatar(typeImageUrl);

                if (texture != null)
                {
                    // Create sprite from texture
                    var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    
                    if (playerTypeIcon != null)
                    {
                        playerTypeIcon.sprite = sprite;
                        playerTypeIcon.gameObject.SetActive(true);
                    }
                }
                else
                {
                    // Show default or hide icon
                    if (playerTypeIcon != null)
                    {
                        playerTypeIcon.gameObject.SetActive(true);
                        // You could set a default type icon sprite here
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error loading type image: {e.Message}");
                if (playerTypeIcon != null)
                    playerTypeIcon.gameObject.SetActive(true);
            }
            finally
            {
                // Hide loading text
                if (typeLoadingText != null)
                    typeLoadingText.gameObject.SetActive(false);
            }
        }
    }
}
