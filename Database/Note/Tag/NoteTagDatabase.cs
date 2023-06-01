namespace Database.Note.Tag;

public class NoteTagDatabase
{
    public int Id { get; set; }
    
    public Guid NoteId { get; set; }
    
    public int TagId { get; set; }
}