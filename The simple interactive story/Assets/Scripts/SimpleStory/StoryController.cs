using System;
using System.Collections;
using System.Collections.Generic;
using Global;
using JetBrains.Annotations;
using SimpleStory.Data;
using UnityEngine;

namespace SimpleStory
{
    public class StoryController
    {
        private readonly Dictionary<string, Scene> _scenes = new Dictionary<string, Scene>();
        private readonly Dictionary<string, Sprite> _images = new Dictionary<string, Sprite>();
        private readonly Dictionary<string, AudioClip> _sounds = new Dictionary<string, AudioClip>();
        [CanBeNull] private readonly Scene _mainScene = null;

        public StoryController() { }
        public StoryController(Scene[] scenes)
        {
            foreach (Scene scene in scenes)
            {
                _scenes[scene.id] = scene;
                if (scene.isMain) _mainScene = scene;
            }
        }

        public string GetWord(string key)
        {
            return key;
        }

        public Scene GetScene(string key=null)
        {
            if (key == null) return _mainScene;
            
            return _scenes.TryGetValue(key, out Scene scene) ? scene : null;
        }

        public IEnumerator LoadSprite(string url, Action<Sprite> answer)
        {
            if (_images.TryGetValue(url, out Sprite image))
            {
                answer(image);
                return Empty();
            }
            
            return UrlReader.ReadSprite(url, (sprite) =>
            {
                if (sprite != null) _images[url] = sprite;
                answer(sprite);
            });
        }

        public IEnumerator LoadClip(string url, Action<AudioClip> answer)
        {
            if (_sounds.TryGetValue(url, out AudioClip sound))
            {
                answer(sound);
                return Empty();
            }
            
            return UrlReader.ReadAudio(url,url, (clip) =>
            {
                if (clip != null) _sounds[url] = clip;
                answer(clip);
            });
        }

        private IEnumerator Empty()
        {
            yield break;
        }
    }
}