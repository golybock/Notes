using Database.User;
using Npgsql;

namespace Repositories.Repositories.Readers.User;

public class UserReader : IReader<UserDatabase>
{
    private const string Id = "id";
    private const string Email = "email";
    private const string PasswordHash = "password_hash";
    
    public static async Task<UserDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            UserDatabase userDatabase = new UserDatabase();
            
            userDatabase.Id = reader.GetGuid(reader.GetOrdinal(Id));
            userDatabase.Email = reader.GetString(reader.GetOrdinal(Email));
                
            var passwordHash = reader.GetValue(reader.GetOrdinal(PasswordHash));

            if (passwordHash != DBNull.Value)
                userDatabase.PasswordHash = passwordHash.ToString();
            
            return userDatabase;
        }

        return null;
    }

    public static async Task<List<UserDatabase>> ReadListAsync(NpgsqlDataReader reader)
    {
        List<UserDatabase> userDatabases = new List<UserDatabase>();

        while (await reader.ReadAsync())
        {
            UserDatabase userDatabase = new UserDatabase();
            
            userDatabase.Id = reader.GetGuid(reader.GetOrdinal(Id));
            userDatabase.Email = reader.GetString(reader.GetOrdinal(Email));
                
            var passwordHash = reader.GetValue(reader.GetOrdinal(PasswordHash));

            if (passwordHash != DBNull.Value)
                userDatabase.PasswordHash = passwordHash.ToString();
            
            userDatabases.Add(userDatabase);
        }

        return userDatabases;
    }
}