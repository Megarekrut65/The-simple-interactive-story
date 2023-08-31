using Global;
using Global.Json;
using Global.Localization;
using Story.Data;
using UnityEngine;

namespace Story
{
    public class StoryController
    {
        private readonly JsonStoryParser _parser;
        private readonly LocalizationManagerInstance _localization;

        public Configuration Config { get; private set; }
        public StoryController(string storyId, string language)
        {
            string path = $"{Application.streamingAssetsPath}/{storyId}";
            Config = JsonObjectParser<Configuration>.Parse($"{path}/config");
            
            _parser = new JsonStoryParser(path, Config.mainFrame);
            _localization = new LocalizationManagerInstance($"{path}/Languages", language);
        }

        public string GetWord(string key)
        {
            return _localization.GetWord(key);
        }

        public Frame GetFrame(string key)
        {
            return _parser.Frames.TryGetValue(key, out var frame) ? frame : null;
        }

        public Sprite GetSprite(string key)
        {
            return FileReader.ReadSprite($"{Application.streamingAssetsPath}/{Config.id}/{key}.png");
        }
    }
}