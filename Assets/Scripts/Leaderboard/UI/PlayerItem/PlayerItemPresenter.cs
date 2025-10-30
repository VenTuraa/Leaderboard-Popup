using System.Threading.Tasks;
using UnityEngine;

namespace Leaderboard
{
    public class PlayerItemPresenter
    {
        private readonly IPlayerItemView _view;

        public PlayerItemPresenter(IPlayerItemView view)
        {
            _view = view;
        }

        public async Task Initialize(PlayerData playerData, int rank)
        {
            _view.SetRank(rank);
            _view.SetPlayerName(playerData.Name);
            _view.SetScore(playerData.Score);

            ApplyPlayerTypeStyling(playerData.Type);

            Task avatarTask = LoadAndApplyAvatar(playerData.Avatar);
            Task typeIconTask = LoadAndApplyTypeIcon(playerData.TypeImageUrl);

            await Task.WhenAll(avatarTask, typeIconTask);
        }

        private void ApplyPlayerTypeStyling(PlayerType playerType)
        {
            Color typeColor = GetPlayerTypeColor(playerType);
            _view.SetNameColor(typeColor);
            _view.SetScoreColor(typeColor);
        }

        private async Task LoadAndApplyAvatar(string avatarUrl)
        {
            _view.ShowAvatarLoading(true);
            _view.ShowAvatar(false);

            Texture2D texture ;
            try
            {
                texture = await AvatarService.Instance.LoadAvatar(avatarUrl);
            }
            finally
            {
                _view.ShowAvatarLoading(false);
            }

            if (texture)
            {
                var sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
                _view.SetAvatarSprite(sprite);
            }

            _view.ShowAvatar(true);
        }

        private async Task LoadAndApplyTypeIcon(string typeImageUrl)
        {
            _view.ShowTypeLoading(true);
            _view.ShowTypeIcon(false);

            Texture2D texture;
            try
            {
                texture = await AvatarService.Instance.LoadAvatar(typeImageUrl);
            }
            finally
            {
                _view.ShowTypeLoading(false);
            }

            if (texture)
            {
                var sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
                _view.SetTypeIconSprite(sprite);
            }
            _view.ShowTypeIcon(true);
        }

        private Color GetPlayerTypeColor(PlayerType playerType)
        {
            return playerType switch
            {
                PlayerType.Diamond => new Color(0f, 1f, 1f, 1f),
                PlayerType.Gold => new Color(1f, 0.843f, 0f, 1f),
                PlayerType.Silver => new Color(0.753f, 0.753f, 0.753f, 1f),
                PlayerType.Bronze => new Color(0.804f, 0.498f, 0.196f, 1f),
                _ => Color.white
            };
        }
    }
}


