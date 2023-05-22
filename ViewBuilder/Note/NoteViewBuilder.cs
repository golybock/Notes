using Domain.Note;
using Domain.Note.Tag;
using ViewBuilder.Note.Tag;
using Views.Note;
using Views.Note.Tag;

namespace ViewBuilder.Note;

public static class NoteViewBuilder
{
    public static NoteView Create(NoteDomain noteDomain)
    {
        return new NoteView()
        {
            Header = noteDomain.Header,
            UserId = noteDomain.UserId,
            Text = noteDomain.Text,
            CreationDate = noteDomain.CreationDate,
            EditedDate = noteDomain.EditedDate,
            Guid = noteDomain.Guid,
            Tags = noteDomain.Tags
                .Select(TagViewBuilder.Create)
                .ToList()
        };
    }

    public static NoteView Create(NoteDomain noteDomain, string text)
    {
        return new NoteView()
        {
            Header = noteDomain.Header,
            Text = text,
            UserId = noteDomain.UserId,
            CreationDate = noteDomain.CreationDate,
            EditedDate = noteDomain.EditedDate,
            Guid = noteDomain.Guid,
            Tags = noteDomain.Tags
                .Select(TagViewBuilder.Create)
                .ToList()
        };
    }

    public static NoteView Create(NoteDomain noteDomain, string text, List<TagDomain> tagDomains)
    {
        return new NoteView()
        {
            Header = noteDomain.Header,
            Text = text,
            UserId = noteDomain.UserId,
            CreationDate = noteDomain.CreationDate,
            EditedDate = noteDomain.EditedDate,
            Guid = noteDomain.Guid,
            Tags = tagDomains
                .Select(TagViewBuilder.Create)
                .ToList()
        };
    }

    public static NoteView Create(NoteDomain noteDomain, string text, List<TagView> tagViews)
    {
        return new NoteView()
        {
            Header = noteDomain.Header,
            Text = text,
            UserId = noteDomain.UserId,
            CreationDate = noteDomain.CreationDate,
            EditedDate = noteDomain.EditedDate,
            Guid = noteDomain.Guid,
            Tags = tagViews
        };
    }
}