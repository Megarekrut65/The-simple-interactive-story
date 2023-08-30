using System;

namespace Story.Data
{
    [Serializable]
    public class Frame
    {
        public string id;
        
        public string background;
        public string textKey;
        
        public string characterLeft;//900x700
        public string characterRight;
        
        public Answer[] answers;
    }
}