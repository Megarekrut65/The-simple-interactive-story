using Global;
using Global.Json;
using Global.Localization;
using SimpleStory.Data;
using UnityEngine;

namespace SimpleStory
{
    public class StoryController
    {
        //private readonly JsonStoryParser _parser;
        private readonly LocalizationManagerInstance _localization;

        //public Configuration Config { get; private set; }

        private readonly string _path;
        
        public StoryController(string storyId, string language)
        {
            _path = $"{Application.streamingAssetsPath}/{storyId}";
            //Config = JsonObjectParser<Configuration>.Parse($"{_path}/config");
            
            //_parser = new JsonStoryParser(_path, Config.mainFrame);
            _localization = new LocalizationManagerInstance($"{_path}/Languages", language);
        }

        public string GetWord(string key)
        {
            return _localization.GetWord(key);
        }

        public Scene GetFrame(string key)
        {
            //return _parser.Frames.TryGetValue(key, out var frame) ? frame : null;
            return null;
        }

        public Sprite GetSprite(string key)
        {
            return FileReader.ReadSprite($"{_path}/{key}.png");
        }

        public AudioClip GetClip(string name)
        {
            return FileReader.ReadAudio(name, $"{_path}/Sounds/{name}.mp3");
        }
    }
}