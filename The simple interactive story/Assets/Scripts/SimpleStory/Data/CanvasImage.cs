namespace SimpleStory.Data
{
    public class Rect
    {
        public double x;
        public double y;
        public double width;
        public double height;
        
        public override string ToString()
        {
            return "Rect:\n" +
                   $"x: {x}\n" +
                   $"y: {y}\n" +
                   $"width: {width}\n" +
                   $"height: {height}\n";
        }
    }
    public class CanvasImage
    {
        public string img;
        public Rect rect;
        
        public override string ToString()
        {
            return "CanvasImage:\n" +
                   $"img: {img}\n" +
                   $"rect: {rect?.ToString() ?? "null"}\n";
        }
    }
}