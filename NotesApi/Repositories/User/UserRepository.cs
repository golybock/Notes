using Database.Note;
using Database.User;
using NotesApi.Repositories.Interfaces.User;
using Npgsql;

namespace NotesApi.Repositories.User;

public class UserRepository : RepositoryBase, IUserRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<UserDatabase?> Get(int id)
    {
        UserDatabase userDatabase = new UserDatabase();

        string query = "select * from user where id == $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter(){ Value = id} }
            };

            await using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                userDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
                userDatabase.Email = reader.GetString(reader.GetOrdinal("email"));
                
                var passwordHash = reader.GetValue(reader.GetOrdinal("email"));

                if (passwordHash != DBNull.Value)
                    userDatabase.PasswordHash = passwordHash.ToString();
            }

            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<UserDatabase?> Get(string email)
    {
        UserDatabase userDatabase = new UserDatabase();

        string query = "select * from user where email == $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter(){ Value = email} }
            };

            await using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                userDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
                userDatabase.Email = reader.GetString(reader.GetOrdinal("email"));
                
                var passwordHash = reader.GetValue(reader.GetOrdinal("email"));

                if (passwordHash != DBNull.Value)
                    userDatabase.PasswordHash = passwordHash.ToString();
                
                
            }

            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}