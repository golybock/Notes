namespace Database.Note;

public class SharedNoteDatabase
{
    public int Id { get; set; }

    public Guid NoteId { get; set; }
    
    public Guid UserId { get; set; }
    
    public int PermissionsLevelId { get; set; }
}