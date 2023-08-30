using System;
using UnityEngine;

namespace Story
{
    public class StoryLoader : MonoBehaviour
    {
        [SerializeField] private string mainFrame;
        private void Start()
        {
            JsonStoryParser parser = new JsonStoryParser(mainFrame);
            var dic = parser.Frames;
            foreach (var item in dic)
            {
                Debug.Log($"{item.Value.id}, {item.Value.textKey}, " +
                          $"{item.Value.background}, {item.Value.characterLeft}, {item.Value.characterRight}");
                if(item.Value.answers == null) continue;
                foreach (var ans in item.Value.answers)
                {
                    Debug.Log($"{ans.textKey}, {ans.action}, {ans.nextFrameId}");
                }
            }
        }
    }
}