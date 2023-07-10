using Database.Note;
using Domain.Note;

namespace DomainBuilder.Note;

public class NoteImageDomainBuilder
{
    public static NoteImageDomain Create(NoteImageDatabase imageNoteDatabase)
    {
        return new NoteImageDomain()
        {
            Id = imageNoteDatabase.Id,
            NoteId = imageNoteDatabase.NoteId,
            X = imageNoteDatabase.X,
            Y = imageNoteDatabase.Y,
            Width = imageNoteDatabase.Width,
            Height = imageNoteDatabase.Height
        };
    }    
}