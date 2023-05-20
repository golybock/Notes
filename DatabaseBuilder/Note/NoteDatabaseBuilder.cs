using Blank.Note;
using Database.Note;

namespace DatabaseBuilder.Note;

public static class NoteDatabaseBuilder
{
    public static NoteDatabase Create(int id, NoteBlank noteBlank)
    {
        return new NoteDatabase()
        {
            Id = id,
            Header = noteBlank.Header
        };
    }
    
    public static NoteDatabase Create(int id, string sourcePath, NoteBlank noteBlank)
    {
        return new NoteDatabase()
        {
            Id = id,
            Header = noteBlank.Header,
            SourcePath = sourcePath
        };
    }
    
    public static NoteDatabase Create(int id, string sourcePath, NoteBlank noteBlank, int userId)
    {
        return new NoteDatabase()
        {
            Id = id,
            Header = noteBlank.Header,
            SourcePath = sourcePath,
            UserId = userId
        };
    }
    
    public static NoteDatabase Create(NoteBlank noteBlank, int userId)
    {
        return new NoteDatabase()
        {
            Header = noteBlank.Header,
            UserId = userId
        };
    }
}