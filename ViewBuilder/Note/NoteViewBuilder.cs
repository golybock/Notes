using Domain.Note;
using ViewBuilder.Note.Tag;
using ViewBuilder.User;
using Views.Note;

namespace ViewBuilder.Note;

public static class NoteViewBuilder
{
    public static NoteView Create(NoteDomain noteDomain)
    {
        var note = new NoteView();

        note.Id = noteDomain.Id;
        
        note.Header = noteDomain.Header;
        
        note.Text = noteDomain.Text;
        
        note.CreationDate = noteDomain.CreationDate;
        
        note.EditedDate = noteDomain.EditedDate;
        
        if (noteDomain.Type != null)
            note.Type = NoteTypeViewBuilder.Create(noteDomain.Type);
        
        if (noteDomain.OwnerUser != null)
            note.OwnerUser = UserViewBuilder.Create(noteDomain.OwnerUser);
        
        note.SharedUsers = noteDomain.SharedUsers
            .Select(UserViewBuilder.Create)
            .ToList();
        
        note.Tags = noteDomain.Tags
            .Select(TagViewBuilder.Create)
            .ToList();

        note.Images = noteDomain.Images
            .Select(NoteImageViewBuilder.Create)
            .ToList();

        return note;
    }
}