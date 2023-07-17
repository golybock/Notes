namespace Blank.Note;

public class NoteBlank
{
    public string Header { get; set; }
    
    public string? Text { get; set; }
    
    public List<NoteImageBlank> Images { get; set; } = new List<NoteImageBlank>();
    
    public List<Guid> Tags { get; set; } = new List<Guid>();
}