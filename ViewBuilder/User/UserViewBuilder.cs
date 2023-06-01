using Domain.Note;
using Domain.User;
using Views.Note;
using Views.User;

namespace ViewBuilder.User;

public static class UserViewBuilder
{
    public static UserView? Create(UserDomain? userDomain)
    {
        if (userDomain == null)
            return null;

        return new UserView()
        {
            Email = userDomain.Email,
            Name = userDomain.Name
        };
    }
}