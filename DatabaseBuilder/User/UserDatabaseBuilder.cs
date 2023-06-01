using Blank.User;
using Database.User;

namespace DatabaseBuilder.User;

public static class UserDatabaseBuilder
{
    public static UserDatabase Create(UserBlank userBlank)
    {
        return new UserDatabase()
        {
            Name = userBlank.Name,
            Email = userBlank.Email
        };
    } 
    
    public static UserDatabase Create(UserBlank userBlank, string hashedPassword)
    {
        return new UserDatabase()
        {
            Name = userBlank.Name,
            Email = userBlank.Email,
            PasswordHash = hashedPassword
        };
    } 
    
    public static UserDatabase Create(int id, UserBlank userBlank)
    {
        return new UserDatabase()
        {
            Id = id,
            Name = userBlank.Name,
            Email = userBlank.Email
        };
    } 
    
    public static UserDatabase Create(int id, UserBlank userBlank, string hashedPassword)
    {
        return new UserDatabase()
        {
            Id = id,
            Name = userBlank.Name,
            Email = userBlank.Email,
            PasswordHash = hashedPassword
        };
    } 
}