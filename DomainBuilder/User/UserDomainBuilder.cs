using Database.User;
using Domain.Note;
using Domain.User;

namespace DomainBuilder.User;

public static class UserDomainBuilder
{
    public static UserDomain Create(UserDatabase userDatabase)
    {
        return new UserDomain()
        {
            Id = userDatabase.Id,
            PasswordHash = userDatabase.PasswordHash,
            Email = userDatabase.Email,
        };
    }
}