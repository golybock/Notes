using Domain.Note;

namespace Domain.User;

public class NoteUserDomain
{
    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public List<NoteDomain> Notes { get; set; } = new List<NoteDomain>();
}