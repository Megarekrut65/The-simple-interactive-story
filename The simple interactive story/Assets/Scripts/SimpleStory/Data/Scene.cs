using System;

namespace SimpleStory.Data
{
    public class Scene
    {
        public string id;
        public string title;
        public string text;
        public bool isMain;
        
        public string background;
        public string music;

        public Answer[] answers;
        public CanvasImage[] images;
        
        public override string ToString()
        {
            string answersString = answers != null ? string.Join("\n", Array.ConvertAll(answers, a => a.ToString())) : "null";
            string imagesString = images != null ? string.Join("\n", Array.ConvertAll(images, i => i.ToString())) : "null";

            return "Scene:\n" +
                   $"id: {id}\n" +
                   $"title: {title}\n" +
                   $"text: {text}\n" +
                   $"isMain: {isMain}\n" +
                   $"background: {background}\n" +
                   $"music: {music}\n" +
                   $"answers: {answersString}\n" +
                   $"images: {imagesString}\n";
        }
    }
}