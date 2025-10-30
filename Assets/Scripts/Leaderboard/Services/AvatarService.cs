using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Leaderboard
{
    public class AvatarService
    {
        private static AvatarService _instance;
        public static AvatarService Instance => _instance ??= new AvatarService();

        private readonly Dictionary<string, Texture2D> _avatarCache = new();
        private readonly Dictionary<string, Task<Texture2D>> _loadingTasks = new();

        public async Task<Texture2D> LoadAvatar(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            if (_avatarCache.TryGetValue(url, out Texture2D cachedTexture))
                return cachedTexture;

            if (_loadingTasks.TryGetValue(url, out Task<Texture2D> loadingTask))
                return await loadingTask;

            var task = LoadAvatarFromUrl(url);
            _loadingTasks[url] = task;

            try
            {
                Texture2D texture = await task;
                if (texture)
                {
                    _avatarCache[url] = texture;
                }
                return texture;
            }
            finally
            {
                _loadingTasks.Remove(url);
            }
        }

        private async Task<Texture2D> LoadAvatarFromUrl(string url)
        {
            using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                return DownloadHandlerTexture.GetContent(request);
            }

            Debug.LogError($"Failed to load avatar from {url}: {request.error}");
            return null;
        }

        public void ClearCache()
        {
            foreach (var texture in _avatarCache.Values)
            {
                if (texture)
                    Object.Destroy(texture);
            }
            _avatarCache.Clear();
            _loadingTasks.Clear();
        }
    }
}
