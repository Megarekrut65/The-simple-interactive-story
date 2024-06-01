using System;

namespace Network.Data
{
    [Serializable]
    public class ImageFields
    {
        public FirestoreString img;
        public FirestoreString name;
        public FirestoreString id;
    }
    [Serializable]
    public class SoundFields
    {
        public FirestoreString sound;
        public FirestoreString name;
        public FirestoreString id;
    }
    
    [Serializable]
    public class StoryFields
    {
        public FirestoreString id;
        public FirestoreString title;
        public FirestoreString description;
        
        public FirestoreString authorId;
        public FirestoreString author;
        
        public FirestoreMap<ImageFields> banner;
        public FirestoreString font;

        public FirestoreTimestamp creatingDate;
        public FirestoreString publish;
    }
    
    [Serializable]
    public class PublishFields
    {
        public FirestoreString id;
        public FirestoreString authorId;
        public FirestoreString storyId;
        public FirestoreTimestamp publishDate;
        public FirestoreBoolean privateStory;
    }

    [Serializable]
    public class AnswerFields
    {
        public FirestoreString id;
        public FirestoreString text;
        public FirestoreString nextScene;
    }

    [Serializable]
    public class RectFields
    {
        public FirestoreDouble x;
        public FirestoreDouble y;
        public FirestoreDouble width;
        public FirestoreDouble height;
        public FirestoreDouble rotation;
    }

    [Serializable]
    public class CanvasImageFields
    {
        public FirestoreMap<ImageFields> img;
        public FirestoreMap<RectFields> rect;
    }

    [Serializable]
    public class SceneFields
    {
        public FirestoreString id;
        public FirestoreString title;
        public FirestoreString text;
        public FirestoreBoolean isMain;
        
        public FirestoreMap<ImageFields> background;
        public FirestoreMap<SoundFields> music;

        public FirestoreArray<AnswerFields> answers;
        public FirestoreArray<CanvasImageFields> images;
    }
}