using Views.Note;

namespace Views.User;

public class UserView
{
    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public List<NoteView> Notes { get; set; } = new List<NoteView>();
}