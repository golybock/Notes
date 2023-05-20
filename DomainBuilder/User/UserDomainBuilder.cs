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
            Email = userDatabase.Email,
            Name = userDatabase.Name
        };
    }
    
    public static UserDomain Create(UserDatabase userDatabase, List<NoteDomain> noteDomains)
    {
        return new UserDomain()
        {
            Email = userDatabase.Email,
            Name = userDatabase.Name,
            Notes = noteDomains
        };
    }
}