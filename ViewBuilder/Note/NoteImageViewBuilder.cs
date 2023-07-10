using Domain.Note;
using Views.Note;

namespace ViewBuilder.Note;

public class NoteImageViewBuilder
{
    public static NoteImageView Create(NoteImageDomain imageNoteDatabase)
    {
        return new NoteImageView()
        {
            Id = imageNoteDatabase.Id,
            X = imageNoteDatabase.X,
            Y = imageNoteDatabase.Y,
            Width = imageNoteDatabase.Width,
            Height = imageNoteDatabase.Height
        };
    }  
}