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
            Id = noteDomain.Id,
            Header = noteDomain.Header,
            UserId = noteDomain.UserId,
            Text = noteDomain.Text,
            CreationDate = noteDomain.CreationDate,
            LastEditDate = noteDomain.LastEditDate,
            Tags = noteDomain.Tags
                .Select(TagViewBuilder.Create)
                .ToList()
        };
    }

    public static NoteView Create(NoteDomain noteDomain, string text)
    {
        return new NoteView()
        {
            Id = noteDomain.Id,
            Header = noteDomain.Header,
            Text = text,
            UserId = noteDomain.UserId,
            CreationDate = noteDomain.CreationDate,
            LastEditDate = noteDomain.LastEditDate,
            Tags = noteDomain.Tags
                .Select(TagViewBuilder.Create)
                .ToList()
        };
    }

    public static NoteView Create(NoteDomain noteDomain, string text, List<TagDomain> tagDomains)
    {
        return new NoteView()
        {
            Id = noteDomain.Id,
            Header = noteDomain.Header,
            Text = text,
            UserId = noteDomain.UserId,
            CreationDate = noteDomain.CreationDate,
            LastEditDate = noteDomain.LastEditDate,
            Tags = tagDomains
                .Select(TagViewBuilder.Create)
                .ToList()
        };
    }

    public static NoteView Create(NoteDomain noteDomain, string text, List<TagView> tagViews)
    {
        return new NoteView()
        {
            Id = noteDomain.Id,
            Header = noteDomain.Header,
            Text = text,
            UserId = noteDomain.UserId,
            CreationDate = noteDomain.CreationDate,
            LastEditDate = noteDomain.LastEditDate,
            Tags = tagViews
        };
    }
}