using Domain.Note;
using Domain.User;
using ViewBuilder.Note;
using Views.Note;
using Views.User;

namespace ViewBuilder.User;

public static class NoteUserViewBuilder
{
    public static NoteUserView Create(NoteUserDomain noteUserDatabase)
    {
        return new NoteUserView()
        {
            Email = noteUserDatabase.Email,
            Name = noteUserDatabase.Name
        };
    }

    public static NoteUserView Create(NoteUserDomain noteUserDatabase, List<NoteView> noteViews)
    {
        return new NoteUserView()
        {
            Email = noteUserDatabase.Email,
            Name = noteUserDatabase.Name,
            Notes = noteViews
        };
    }

    public static NoteUserView Create(NoteUserDomain noteUserDatabase, List<NoteDomain> noteDomains)
    {
        return new NoteUserView()
        {
            Email = noteUserDatabase.Email,
            Name = noteUserDatabase.Name,
            Notes = noteDomains
                .Select(NoteViewBuilder.Create)
                .ToList()
        };
    }
}