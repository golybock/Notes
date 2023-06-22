using System.ComponentModel.DataAnnotations;
using Blank.Note.Layers;
using Blank.Note.Tag;

namespace Blank.Note;

public class NoteBlank
{
    public string Header { get; set; } = string.Empty;
    
    public string? Text { get; set; }
    
    public List<ImageNoteBlank> Images { get; set; } = new List<ImageNoteBlank>();
    
    public List<Guid> Tags { get; set; } = new List<Guid>();
}