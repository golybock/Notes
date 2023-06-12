using Domain.Note.Tag;
using Views.Note.Tag;

namespace ViewBuilder.Note;

public class NoteTypeViewBuilder
{
    public static NoteTypeView Create(NoteTypeDomain noteDomain)
    {
        return new NoteTypeView()
        {
            Name = noteDomain.Name
        };
    }
}