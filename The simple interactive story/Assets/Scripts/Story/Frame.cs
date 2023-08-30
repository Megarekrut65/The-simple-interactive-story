using System;

namespace Story
{
    [Serializable]
    public class Frame
    {
        public string id;
        
        public string background;
        public string textKey;
        
        public string characterLeft;
        public string characterRight;
        
        public Answer[] answers;
    }
}