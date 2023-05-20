namespace Database.Note;

public class NoteDatabase
{
    public int Id { get; set; }

    public string Header { get; set; } = null!;
    
    public DateTime CreationDate { get; set; }
    
    public DateTime LastEditDate { get; set; }
    
    public string? SourcePath { get; set; }
    
    public int UserId { get; set; }
}