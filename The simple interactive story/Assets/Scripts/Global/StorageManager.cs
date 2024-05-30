using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Global
{
    public class StorageManager: MonoBehaviour
    {
        private readonly Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
        private readonly Dictionary<string, AudioClip> _clips = new Dictionary<string, AudioClip>();
        
        public static StorageManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        public static void Save(string url, Sprite sprite)
        {
            if (Instance == null) return;
            Instance._sprites[url] = sprite;
        }
        public static void Save(string url, AudioClip clip)
        {
            if (Instance == null) return;
            Instance._clips[url] = clip;
        }
        
        [CanBeNull]
        public static Sprite GetSavedSprite(string url)
        {
            if (Instance == null) return null;
            return Instance._sprites.TryGetValue(url, out Sprite sprite) ? sprite : null;
        }
        [CanBeNull]
        public static AudioClip GetSavedClip(string url)
        {
            if (Instance == null) return null;
            return Instance._clips.TryGetValue(url, out AudioClip clip) ? clip : null;
        }

        public static bool ContainsSprite(string url)
        {
            if (Instance == null) return false;

            return Instance._sprites.ContainsKey(url);
        }
        public static bool ContainsClip(string url)
        {
            if (Instance == null) return false;

            return Instance._clips.ContainsKey(url);
        }
    }
}