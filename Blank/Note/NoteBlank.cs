using System.ComponentModel.DataAnnotations;

namespace Blank.Note;

public class NoteBlank
{
    [Required]
    public string Header { get; set; } = null!;
    
    public string? Text { get; set; }

    // list of guid tags
    public List<Guid> Tags { get; set; } = new List<Guid>();
}