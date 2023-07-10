namespace Views.Note;

public class NoteImageView
{
    public Guid Id { get; set; }
    
    public int X { get; set; }
    
    public int Y { get; set; }
    
    public int Width { get; set; }
    
    public int Height { get; set; }

    public string? Url { get; set; }
}