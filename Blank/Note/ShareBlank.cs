using System.ComponentModel.DataAnnotations;

namespace Blank.Note;

public class ShareBlank
{
    [Required]
    public Guid NoteId { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Range(1, 3)]
    public int PermissionLevel { get; set; }
}