using Views.Note.Tag;

namespace Views.Note;

public class NoteView
{
    public string Header { get; set; } = null!;
    
    public DateTime CreationDate { get; set; }
    
    public DateTime EditedDate { get; set; }
    
    public string? Text { get; set; }
    
    public int UserId { get; set; }
    
    public Guid Guid { get; set; }

    public List<TagView> Tags { get; set; } = new List<TagView>();
}