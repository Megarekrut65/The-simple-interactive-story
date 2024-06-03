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
        private readonly Dictionary<string, bool> _loadedScenes = new Dictionary<string, bool>();
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

        public bool IsSceneLoaded(Scene scene) => _loadedScenes.TryGetValue(scene.id, out bool value) && value;

        public IEnumerator LoadScene(Scene scene, int depth=2)
        {
            if (scene == null) yield break;
            _loadedScenes[scene.id] = true;
            
            List<IEnumerator> enumerators = new List<IEnumerator>();
            if(scene.background != null) enumerators.Add(LoadSprite(scene.background));
            
            if(scene.music != null) enumerators.Add(LoadClip(scene.music));

            if (scene.images != null)
                enumerators.AddRange(scene.images.Select(img => LoadSprite(img.img)));
            
            if (scene.answers != null && depth >= 0)
            {
                enumerators.AddRange(from answer in scene.answers 
                    where answer.nextScene != scene.id && answer.nextScene != null
                    select LoadScene(_scenes.TryGetValue(answer.nextScene, out Scene nextScene)?nextScene:null, depth-1));
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
            return StorageManager.GetSavedSprite(url);
        }

        public AudioClip GetClip(string url)
        {
            return StorageManager.GetSavedClip(url);
        }

        private IEnumerator LoadSprite(string url)
        {
            if (StorageManager.ContainsSprite(url) || url == null || url == "")
                return Empty();
            
            return UrlReader.ReadSprite(url, (sprite) =>
            {
                if (sprite != null) StorageManager.Save(url, sprite);
            });
        }

        private IEnumerator LoadClip(string url)
        {
            if (StorageManager.ContainsClip(url) || url == null || url == "")
                return Empty();
            
            
            return UrlReader.ReadAudio(url,url, (clip) =>
            {
                if (clip != null) StorageManager.Save(url, clip);
            });
        }
        
        private IEnumerator Empty()
        {
            yield break;
        }
    }
}