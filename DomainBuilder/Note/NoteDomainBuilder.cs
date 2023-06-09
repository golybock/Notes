using Database.Note;
using Database.User;
using Domain.Note;
using DomainBuilder.User;

namespace DomainBuilder.Note;

public static class NoteDomainBuilder
{
    public static NoteDomain Create(NoteDatabase noteDatabase)
    {
        return new NoteDomain()
        {
            Id = noteDatabase.Id,
            Header = noteDatabase.Header,
            CreationDate = noteDatabase.CreationDate,
            EditedDate = noteDatabase.EditedDate
        };
    }
    
    public static NoteDomain Create(NoteDatabase noteDatabase, NoteTypeDatabase? noteTypeDatabase)
    {
        return new NoteDomain()
        {
            Id = noteDatabase.Id,
            Header = noteDatabase.Header,
            CreationDate = noteDatabase.CreationDate,
            EditedDate = noteDatabase.EditedDate,
            Type = NoteTypeDomainBuilder.Create(noteTypeDatabase)
        };
    }
    
    public static NoteDomain Create(NoteDatabase noteDatabase, NoteTypeDatabase? noteTypeDatabase, string? text)
    {
        return new NoteDomain()
        {
            Id = noteDatabase.Id,
            Header = noteDatabase.Header,
            Text = text,
            CreationDate = noteDatabase.CreationDate,
            EditedDate = noteDatabase.EditedDate,
            Type = NoteTypeDomainBuilder.Create(noteTypeDatabase)
        };
    }

    public static NoteDomain Create(NoteDatabase noteDatabase, string text)
    {
        return new NoteDomain()
        {
            Id = noteDatabase.Id,
            Header = noteDatabase.Header,
            Text = text,
            CreationDate = noteDatabase.CreationDate,
            EditedDate = noteDatabase.EditedDate
        };
    }
    
    public static NoteDomain Create(NoteDatabase noteDatabase, List<UserDatabase> sharedUsers)
    {
        return new NoteDomain()
        {
            Id = noteDatabase.Id,
            Header = noteDatabase.Header,
            CreationDate = noteDatabase.CreationDate,
            EditedDate = noteDatabase.EditedDate,
            SharedUsers = sharedUsers.Select(UserDomainBuilder.Create).ToList(),
        };
    }
    
    public static NoteDomain Create(NoteDatabase noteDatabase, string text, List<UserDatabase> sharedUsers, List<NoteImageDatabase> images)
    {
        return new NoteDomain()
        {
            Id = noteDatabase.Id,
            Header = noteDatabase.Header,
            Text = text,
            CreationDate = noteDatabase.CreationDate,
            EditedDate = noteDatabase.EditedDate,
            SharedUsers = sharedUsers.Select(UserDomainBuilder.Create).ToList(),
            Images = images.Select(NoteImageDomainBuilder.Create).ToList()
        };
    }
}