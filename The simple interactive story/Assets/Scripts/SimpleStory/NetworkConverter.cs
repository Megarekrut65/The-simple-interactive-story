using System.Linq;
using Network.Data;
using SimpleStory.Data;

namespace SimpleStory
{
    public static class NetworkConverter
    {
        public static string Convert(FirestoreString value) => value?.stringValue;
        public static string Convert(FirestoreTimestamp value) => value?.timestampValue;
        public static double Convert(FirestoreDouble value) => value?.doubleValue ?? 0;
        public static bool Convert(FirestoreBoolean value) => value?.booleanValue ?? false;
        
        public static TFields ConvertMap<TFields>(FirestoreMap<TFields> value) where TFields: class 
            => value?.mapValue?.fields;

        public static TFields[] ConvertArray<TFields>(FirestoreArray<TFields> value) where TFields : class
            => value?.arrayValue?.values?.Select(ConvertMap).ToArray();
        public static string Convert(FirestoreMap<ImageFields> value)
            => Convert(ConvertMap(value)?.img);
        public static string Convert(FirestoreMap<SoundFields> value)
            => Convert(ConvertMap(value)?.sound);

        public static Answer Convert(AnswerFields value) => 
            value == null
            ? null
            : new Answer { id = Convert(value.id), text = Convert(value.text), nextScene = Convert(value.nextScene) };
        public static Answer[] Convert(FirestoreArray<AnswerFields> value) 
            => ConvertArray(value)?.Select(Convert).ToArray();

        public static Rect Convert(RectFields value) =>
            value == null
                ? null
                : new Rect { x = Convert(value.x), y = Convert(value.y), 
                    width = Convert(value.width), height = Convert(value.height), rotation = Convert(value.rotation)};

        public static CanvasImage Convert(CanvasImageFields value) =>
            value == null
                ? null
                : new CanvasImage { img = Convert(value.img), rect = Convert(ConvertMap(value.rect)) };
        public static CanvasImage[] Convert(FirestoreArray<CanvasImageFields> value) 
            => ConvertArray(value)?.Select(Convert).ToArray();
        
        public static Story Convert(FirestoreDocument<StoryFields> document)
        {
            StoryFields fields = document.fields ?? new StoryFields();
            Story story = new Story
            {
                id = Convert(fields.id),
                author = Convert(fields.author),
                authorId = Convert(fields.authorId),
                title = Convert(fields.title),
                description = Convert(fields.description),
                banner = Convert(fields.banner),
                publish = Convert(fields.publish),
                font = Convert(fields.font),
                creatingDate = Convert(fields.creatingDate)
            };

            return story;
        }
        public static Publish Convert(FirestoreDocument<PublishFields> document)
        {
            PublishFields fields = document.fields ?? new PublishFields();
            Publish publish = new Publish
            {
                id = Convert(fields.id),
                authorId = Convert(fields.authorId),
                storyId = Convert(fields.storyId),
                publishDate = Convert(fields.publishDate),
                privateStory = Convert(fields.privateStory)
            };

            return publish;
        }
        
        public static Scene Convert(FirestoreDocument<SceneFields> document)
        {
            SceneFields fields = document.fields;
            Scene scene = new Scene
            {
                id = Convert(fields.id),
                title = Convert(fields.title),
                text = Convert(fields.text),
                isMain = Convert(fields.isMain),
                background = Convert(fields.background),
                music = Convert(fields.music),
                answers = Convert(fields.answers),
                images = Convert(fields.images)
            };

            return scene;
        }

        public static Scene[] Convert(FirestoreCollection<SceneFields> collection)
        {
            return collection?.documents?.Select(Convert).ToArray();
        }
    }
}