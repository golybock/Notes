using Domain.Note.Tag;

namespace Domain.Note;

public class NoteDomain
{
    public int Id { get; set; }

    public string Header { get; set; } = null!;
    
    public DateTime CreationDate { get; set; }
    
    public DateTime LastEditDate { get; set; }
    
    public string? Text { get; set; }
    
    public int UserId { get; set; }

    public List<TagDomain> Tags { get; set; } = new List<TagDomain>();
}