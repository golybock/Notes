using Views.Note.Layers;
using Views.Note.Tag;
using Views.User;

namespace Views.Note;

public class NoteView
{
    public Guid Id { get; set; }

    public string Header { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime EditedDate { get; set; }

    public string? Text { get; set; }

    public NoteTypeView? Type { get; set; }

    public UserView? OwnerUser { get; set; }

    public List<UserView> SharedUsers { get; set; } = new List<UserView>();

    public List<ImageNoteView> Images { get; set; } = new List<ImageNoteView>();

    public List<TagView> Tags { get; set; } = new List<TagView>();
}