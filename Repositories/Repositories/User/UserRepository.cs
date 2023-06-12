using Database.User;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Repositories.Repositories.Interfaces.User;
using Repositories.Repositories.Readers.User;

namespace Repositories.Repositories.User;

public class UserRepository : RepositoryBase, IUserRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<UserDatabase?> Get(Guid id)
    {
        string query = "select * from users where id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter(){ Value = id} }
            };

            await using var reader = await command.ExecuteReaderAsync();

            return await UserReader.ReadAsync(reader);
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
        string query = "select * from users where email = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter(){ Value = email} }
            };

            await using var reader = await command.ExecuteReaderAsync();
            
            return await UserReader.ReadAsync(reader);
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

    public async Task<Guid> Create(UserDatabase userDatabase)
    {
        string query = "insert into users(id, email, password_hash)" +
                       "values ($1, $2, $3) returning id";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = userDatabase.Id },
                    new NpgsqlParameter() { Value = userDatabase.Email },
                    new NpgsqlParameter() { Value = userDatabase.PasswordHash },
                }
            };

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                return reader.GetGuid(reader.GetOrdinal("id"));

            return Guid.Empty;
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

    public async Task<bool> Update(Guid id, UserDatabase userDatabase)
    {
        string query = "update users set password_hash = $2 " +
                       "where id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = id },
                    new NpgsqlParameter() { Value = userDatabase.PasswordHash },
                }
            };

            return await command.ExecuteNonQueryAsync() > 0;
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

    public async Task<bool> Update(string email, UserDatabase userDatabase)
    {
        string query = "update users set password_hash = $2 " +
                       "where email = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = email },
                    new NpgsqlParameter() { Value = userDatabase.PasswordHash },
                }
            };

            return await command.ExecuteNonQueryAsync() > 0;
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
    
    public async Task<bool> UpdatePassword(Guid id, string newPassword)
    {
        string query = "update users set password_hash = $2 " +
                       "where id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = id },
                    new NpgsqlParameter() { Value = newPassword }
                }
            };

            return await command.ExecuteNonQueryAsync() > 0;
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

    public async Task<bool> Delete(Guid id)
    {
        return await DeleteAsync("user", "id", id) > 0;
    }
}