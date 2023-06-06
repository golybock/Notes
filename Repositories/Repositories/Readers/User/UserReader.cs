using Database.User;
using Npgsql;

namespace Repositories.Repositories.Readers.User;

public class UserReader : IReader<UserDatabase>
{
    public static async Task<UserDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            UserDatabase userDatabase = new UserDatabase();
            
            userDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
            userDatabase.Email = reader.GetString(reader.GetOrdinal("email"));
                
            var passwordHash = reader.GetValue(reader.GetOrdinal("password_hash"));

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
            
            userDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
            userDatabase.Email = reader.GetString(reader.GetOrdinal("email"));
                
            var passwordHash = reader.GetValue(reader.GetOrdinal("password_hash"));

            if (passwordHash != DBNull.Value)
                userDatabase.PasswordHash = passwordHash.ToString();
        }

        return userDatabases;
    }
}