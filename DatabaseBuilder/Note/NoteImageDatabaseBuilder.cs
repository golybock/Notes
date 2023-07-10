using Blank.Note;
using Database.Note;

namespace DatabaseBuilder.Note;

public class NoteImageDatabaseBuilder
{
    public static NoteImageDatabase? Create(NoteImageBlank noteImageBlank)
    {
        return new NoteImageDatabase()
        {
            Id = noteImageBlank.Id,
            X = noteImageBlank.X,
            Y = noteImageBlank.Y,
            Width = noteImageBlank.Width,
            Height = noteImageBlank.Height,
        };
    }
    
    public static NoteImageDatabase? Create(NoteImageBlank noteImageBlank, Guid noteId)
    {
        return new NoteImageDatabase()
        {
            Id = noteImageBlank.Id,
            NoteId = noteId,
            X = noteImageBlank.X,
            Y = noteImageBlank.Y,
            Width = noteImageBlank.Width,
            Height = noteImageBlank.Height,
        };
    }
}