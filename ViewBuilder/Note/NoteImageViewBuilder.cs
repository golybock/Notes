using Domain.Note;
using Views.Note;

namespace ViewBuilder.Note;

public class NoteImageViewBuilder
{
    public static NoteImageView Create(NoteImageDomain imageDomain)
    {
        return new NoteImageView()
        {
            Id = imageDomain.Id,
            X = imageDomain.X,
            Y = imageDomain.Y,
            Width = imageDomain.Width,
            Height = imageDomain.Height,
            Url = imageDomain.Url
        };
    }  
}