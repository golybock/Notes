namespace Database.Logs;

public class LogsDatabase
{
    public int Id { get; set; }
    
    public DateTime TimeStamp { get; set; }

    public string Action { get; set; } = null!;
    
    public Guid UserId { get; set; }
    
    public Guid? NoteId { get; set; }
}