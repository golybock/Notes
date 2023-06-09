using Domain.Note.Tag;
using Domain.User;

namespace Domain.Note;

public class NoteDomain
{
    public Guid Id { get; set; }

    public string Header { get; set; } = null!;
    
    public DateTime CreationDate { get; set; }
    
    public DateTime EditedDate { get; set; }
    
    public string? Text { get; set; }

    public NoteTypeDomain? Type { get; set; }
    
    public UserDomain? OwnerUser { get; set; }

    public List<UserDomain> SharedUsers { get; set; } = new List<UserDomain>();

    public List<NoteImageDomain> Images { get; set; } = new List<NoteImageDomain>();

    public List<TagDomain> Tags { get; set; } = new List<TagDomain>();
}