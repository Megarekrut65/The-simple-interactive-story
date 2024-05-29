using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerator LoadScene(Scene scene, int depth=2)
        {
            if (scene == null) yield break;
            
            List<IEnumerator> enumerators = new List<IEnumerator>();
            if(scene.background != null) enumerators.Add(LoadSprite(scene.background));
            
            if(scene.music != null) enumerators.Add(LoadClip(scene.music));

            if (scene.images != null)
                enumerators.AddRange(scene.images.Select(img => LoadSprite(img.img)));
            
            if (scene.answers != null && depth >= 0)
            {
                enumerators.AddRange(from answer in scene.answers where answer.nextScene != scene.id 
                    select LoadScene(_scenes[answer.nextScene], depth-1));
            }
            
            foreach (IEnumerator enumerator in enumerators)
            {
                yield return enumerator;
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

        public Sprite GetSprite(string url)
        {
            return _images.TryGetValue(url, out Sprite image) ? image : null;
        }

        public AudioClip GetClip(string url)
        {
            return _sounds.TryGetValue(url, out AudioClip clip) ? clip : null;
        }

        private IEnumerator LoadSprite(string url)
        {
            if (_images.ContainsKey(url))
                return Empty();
            
            return UrlReader.ReadSprite(url, (sprite) =>
            {
                if (sprite != null) _images[url] = sprite;
            });
        }

        private IEnumerator LoadClip(string url)
        {
            if (_sounds.ContainsKey(url))
                return Empty();
            
            
            return UrlReader.ReadAudio(url,url, (clip) =>
            {
                if (clip != null) _sounds[url] = clip;
            });
        }
        
        private IEnumerator Empty()
        {
            yield break;
        }
    }
}