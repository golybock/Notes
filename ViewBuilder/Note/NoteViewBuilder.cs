using Domain.Note;
using Domain.Note.Tag;
using ViewBuilder.Note.Tag;
using ViewBuilder.User;
using Views.Note;
using Views.Note.Tag;

namespace ViewBuilder.Note;

public static class NoteViewBuilder
{
    public static NoteView Create(NoteDomain noteDomain)
    {
        return new NoteView()
        {
            Id = noteDomain.Id,
            Header = noteDomain.Header,
            Text = noteDomain.Text,
            CreationDate = noteDomain.CreationDate,
            EditedDate = noteDomain.EditedDate,
            Type = NoteTypeViewBuilder.Create(noteDomain.Type),
            User = UserViewBuilder.Create(noteDomain.User),
            Tags = noteDomain.Tags
                .Select(TagViewBuilder.Create)
                .ToList()
        };
    }
}