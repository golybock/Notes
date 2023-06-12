using Domain.Note;
using Domain.User;
using Views.Note;
using Views.User;

namespace ViewBuilder.User;

public static class UserViewBuilder
{
    public static UserView Create(UserDomain userDomain)
    {
        return new UserView()
        {
            Id = userDomain.Id,
            Email = userDomain.Email
        };
    }
}