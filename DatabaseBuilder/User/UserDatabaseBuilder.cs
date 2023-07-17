using Blank.User;
using Database.User;

namespace DatabaseBuilder.User;

public static class UserDatabaseBuilder
{
    public static UserDatabase Create(UserBlank userBlank)
    {
        return new UserDatabase()
        {
            Email = userBlank.Email
        };
    } 

    public static UserDatabase Create(UserBlank userBlank, string password)
    {
        return new UserDatabase()
        {
            Email = userBlank.Email,
            PasswordHash = password
        };
    } 
    
    public static UserDatabase Create(Guid id, UserBlank userBlank, string password)
    {
        return new UserDatabase()
        {
            Id = id,
            Email = userBlank.Email,
            PasswordHash = password
        };
    } 
}