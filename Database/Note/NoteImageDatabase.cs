namespace Database.Note;

public class NoteImageDatabase
{
    public Guid Id { get; set; }
    
    public Guid NoteId { get; set; }
    
    public int X { get; set; }
    
    public int Y { get; set; }
    
    public int Width { get; set; }
    
    public int Height { get; set; }
}