using System.ComponentModel.DataAnnotations;
using Blank.Note.Tag;

namespace Blank.Note;

public class NoteBlank
{
    [Required]
    public string Header { get; set; } = null!;
    
    public string? Text { get; set; }

    // list of guid tags
    public List<TagBlank> Tags { get; set; } = new List<TagBlank>();
}