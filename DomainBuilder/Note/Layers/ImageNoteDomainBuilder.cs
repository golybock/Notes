using Database.Note.Layers;
using Domain.Note.Layers;

namespace DomainBuilder.Note.Layers;

public class ImageNoteDomainBuilder
{
    public static ImageNoteDomain Create(ImageNoteDatabase imageNoteDatabase)
    {
        return new ImageNoteDomain()
        {
            Id = imageNoteDatabase.Id,
            X = imageNoteDatabase.X,
            Y = imageNoteDatabase.Y,
            Width = imageNoteDatabase.Width,
            Height = imageNoteDatabase.Height,
            SvgCode = imageNoteDatabase.SvgCode,
            Url = imageNoteDatabase.Url
        };
    } 
}