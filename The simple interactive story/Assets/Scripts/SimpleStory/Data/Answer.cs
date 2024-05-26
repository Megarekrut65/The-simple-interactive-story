namespace SimpleStory.Data
{
    public class Answer
    {
        public string id;
        public string text;
        public string nextScene;
        
        public override string ToString()
        {
            return "Answer:\n" +
                   $"id: {id}\n" +
                   $"text: {text}\n" +
                   $"nextScene: {nextScene}\n";
        }
    }
}