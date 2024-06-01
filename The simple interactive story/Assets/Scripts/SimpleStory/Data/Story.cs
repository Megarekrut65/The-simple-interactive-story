namespace SimpleStory.Data
{
    public class Story
    {
        public string id;
        public string title;
        public string description;
        public string authorId;
        public string author;
        public string banner;
        public string publish;
        public string creatingDate;
        public string font;
        
        public override string ToString()
        {
            return "Story:\n" +
                   $"id: {id}\n" +
                   $"title: {title}\n" +
                   $"description: {description}\n" +
                   $"authorId: {authorId}\n" +
                   $"author: {author}\n" +
                   $"banner: {banner}\n" +
                   $"publish: {publish}\n" +
                   $"creatingDate: {creatingDate}\n" +
                   $"font: {font}\n";
        }
    }
}