using Blank.Note;
using Database.Note;

namespace DatabaseBuilder.Note;

public static class NoteDatabaseBuilder
{
    public static NoteDatabase Create(Guid id, NoteBlank noteBlank)
    {
        return new NoteDatabase()
        {
            Id = id,
            Header = noteBlank.Header
        };
    }
    
    public static NoteDatabase Create(NoteBlank noteBlank)
    {
        return new NoteDatabase()
        {
            Header = noteBlank.Header
        };
    }

    public static NoteDatabase Create(NoteBlank noteBlank, Guid ownerId)
    {
        return new NoteDatabase()
        {
            Header = noteBlank.Header,
            OwnerId = ownerId
        };
    }
}