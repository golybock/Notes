namespace Database.Note;

public class NoteDatabase
{
    public Guid Id { get; set; }

    public string Header { get; set; } = null!;
    
    public DateTime CreationDate { get; set; }
    
    public DateTime EditedDate { get; set; }
    
    public string? SourcePath { get; set; }
    
    public int TypeId { get; set; }
    
    public Guid OwnerId { get; set; }
}