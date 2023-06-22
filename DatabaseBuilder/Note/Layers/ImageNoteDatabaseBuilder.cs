using Blank.Note.Layers;
using Database.Note.Layers;

namespace DatabaseBuilder.Note.Layers;

public class ImageNoteDatabaseBuilder
{
    public static ImageNoteDatabase Create(ImageNoteBlank imageNoteBlank)
    {
        return new ImageNoteDatabase()
        {
            Id = imageNoteBlank.Id,
            X = imageNoteBlank.X,
            Y = imageNoteBlank.Y,
            Width = imageNoteBlank.Width,
            Height = imageNoteBlank.Height,
            SvgCode = imageNoteBlank.SvgCode,
        };
    }
    
    public static ImageNoteDatabase Create(ImageNoteBlank imageNoteBlank, string url)
    {
        return new ImageNoteDatabase()
        {
            Id = imageNoteBlank.Id,
            X = imageNoteBlank.X,
            Y = imageNoteBlank.Y,
            Width = imageNoteBlank.Width,
            Height = imageNoteBlank.Height,
            SvgCode = imageNoteBlank.SvgCode,
            Url = url
        };
    } 
}