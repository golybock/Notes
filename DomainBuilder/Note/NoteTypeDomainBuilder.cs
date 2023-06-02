using Database.Note;
using Domain.Note.Tag;

namespace DomainBuilder.Note;

public class NoteTypeDomainBuilder
{
    public static NoteTypeDomain? Create(NoteTypeDatabase? noteTypeDatabase)
    {
        if (noteTypeDatabase == null)
            return null;
        
        return new NoteTypeDomain()
        {
            Id = noteTypeDatabase.Id,
            Name = noteTypeDatabase.Name
        };
    }
}