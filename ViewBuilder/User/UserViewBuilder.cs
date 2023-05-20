using Domain.Note;
using Domain.User;
using ViewBuilder.Note;
using Views.Note;
using Views.User;

namespace ViewBuilder.User;

public static class UserViewBuilder
{
    public static UserView Create(UserDomain userDatabase)
    {
        return new UserView()
        {
            Email = userDatabase.Email,
            Name = userDatabase.Name
        };
    }

    public static UserView Create(UserDomain userDatabase, List<NoteView> noteViews)
    {
        return new UserView()
        {
            Email = userDatabase.Email,
            Name = userDatabase.Name,
            Notes = noteViews
        };
    }

    public static UserView Create(UserDomain userDatabase, List<NoteDomain> noteDomains)
    {
        return new UserView()
        {
            Email = userDatabase.Email,
            Name = userDatabase.Name,
            Notes = noteDomains
                .Select(NoteViewBuilder.Create)
                .ToList()
        };
    }
}