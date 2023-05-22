using Database.Note;
using Domain.Note;
using Domain.Note.Tag;

namespace DomainBuilder.Note;

public static class NoteDomainBuilder
{
    public static NoteDomain Create(NoteDatabase noteDatabase)
    {
        return new NoteDomain()
        {
            Id = noteDatabase.Id,
            Header = noteDatabase.Header,
            UserId = noteDatabase.UserId,
            CreationDate = noteDatabase.CreationDate,
            EditedDate = noteDatabase.EditedDate,
            Guid = noteDatabase.Guid
        };
    }
    
    public static NoteDomain Create(NoteDatabase noteDatabase, string text)
    {
        return new NoteDomain()
        {
            Id = noteDatabase.Id,
            Header = noteDatabase.Header,
            Text = text,
            UserId = noteDatabase.UserId,
            CreationDate = noteDatabase.CreationDate,
            EditedDate = noteDatabase.EditedDate,
            Guid = noteDatabase.Guid
        };
    }
    
    public static NoteDomain Create(NoteDatabase noteDatabase, string text, List<TagDomain> tagDomains)
    {
        return new NoteDomain()
        {
            Id = noteDatabase.Id,
            Header = noteDatabase.Header,
            Text = text,
            UserId = noteDatabase.UserId,
            CreationDate = noteDatabase.CreationDate,
            EditedDate = noteDatabase.EditedDate,
            Tags = tagDomains,
            Guid = noteDatabase.Guid
        };
    }
}