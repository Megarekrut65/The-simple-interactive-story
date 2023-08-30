using System.Collections.Generic;
using Global;
using JetBrains.Annotations;
using UnityEngine;

namespace Story
{
    public class JsonStoryParser
    {
        public SortedDictionary<string, Frame> Frames { get; private set; } = new SortedDictionary<string, Frame>();

        public JsonStoryParser(string mainFrame)
        {
            ReadFrame(mainFrame);
        }
        private void ReadFrame([CanBeNull] string frameName)
        {
            if(frameName == null || Frames.ContainsKey(frameName)) return;
            
            string path = Application.streamingAssetsPath + "/Frames/" + frameName + ".json";
            
            string jsonData = AllFileReader.Read(path);
            Frame frame = JsonUtility.FromJson<Frame>(jsonData);
            Frames.Add(frame.id, frame);
            
            if(frame.answers == null) return;
            foreach(Answer answer in frame.answers)
            {
                ReadFrame(answer.nextFrameId);
            }
        }
    }
}