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
            OwnerId = noteDatabase.OwnerId,
            TypeId = noteDatabase.TypeId,
            CreationDate = noteDatabase.CreationDate,
            EditedDate = noteDatabase.EditedDate
        };
    }
    
    public static NoteDomain Create(NoteDatabase noteDatabase, string text)
    {
        return new NoteDomain()
        {
            Id = noteDatabase.Id,
            Header = noteDatabase.Header,
            Text = text,
            OwnerId = noteDatabase.OwnerId,
            CreationDate = noteDatabase.CreationDate,
            EditedDate = noteDatabase.EditedDate
        };
    }
    
    public static NoteDomain Create(NoteDatabase noteDatabase, string text, string sourcePath)
    {
        return new NoteDomain()
        {
            Id = noteDatabase.Id,
            Header = noteDatabase.Header,
            Text = text,
            OwnerId = noteDatabase.OwnerId,
            CreationDate = noteDatabase.CreationDate,
            EditedDate = noteDatabase.EditedDate,
            SourcePath = sourcePath
        };
    }
    
    public static NoteDomain Create(NoteDatabase noteDatabase, string text, string sourcePath, List<TagDomain> tagDomains)
    {
        return new NoteDomain()
        {
            Id = noteDatabase.Id,
            Header = noteDatabase.Header,
            Text = text,
            OwnerId = noteDatabase.OwnerId,
            CreationDate = noteDatabase.CreationDate,
            EditedDate = noteDatabase.EditedDate,
            Tags = tagDomains,
            SourcePath = sourcePath
        };
    }
}