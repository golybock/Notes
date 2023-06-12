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

    
    public static NoteDatabase Create(Guid id, string sourcePath, NoteBlank noteBlank)
    {
        return new NoteDatabase()
        {
            Id = id,
            Header = noteBlank.Header,
            SourcePath = sourcePath
        };
    }
    
    public static NoteDatabase Create(Guid id, string sourcePath, NoteBlank noteBlank, Guid ownerId)
    {
        return new NoteDatabase()
        {
            Id = id,
            Header = noteBlank.Header,
            SourcePath = sourcePath,
            OwnerId = ownerId
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