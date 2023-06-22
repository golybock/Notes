using Domain.Note.Layers;
using Views.Note.Layers;

namespace ViewBuilder.Note.Layers;

public static class ImageNoteViewBuilder 
{
    public static ImageNoteView Create(ImageNoteDomain imageNoteDomain)
    {
        return new ImageNoteView()
        {
            Id = imageNoteDomain.Id,
            X = imageNoteDomain.X,
            Y = imageNoteDomain.Y,
            Width = imageNoteDomain.Width,
            Height = imageNoteDomain.Height,
            SvgCode = imageNoteDomain.SvgCode,
            Url = imageNoteDomain.Url
        };
    } 
}