using Database.Note;
using Database.User;
using NotesApi.Repositories.Interfaces.User;
using NotesApi.Repositories.Readers.User;
using Npgsql;

namespace NotesApi.Repositories.User;

public class NoteUserRepository : RepositoryBase, INoteUserRepository
{
    public NoteUserRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<NoteUserDatabase?> Get(int id)
    {
        string query = "select * from note_user where id = $1";

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

    public async Task<NoteUserDatabase?> Get(string email)
    {
        string query = "select * from note_user where email = $1";

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

    public async Task<int> Create(NoteUserDatabase noteUserDatabase)
    {
        string query = "insert into note_user(email, password_hash, name)" +
                       "values ($1, $2, $3) returning id";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = noteUserDatabase.Email },
                    new NpgsqlParameter() { Value = noteUserDatabase.PasswordHash },
                    new NpgsqlParameter() { Value = noteUserDatabase.Name }
                }
            };

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                return reader.GetInt32(reader.GetOrdinal("id"));

            return 1;
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

    public async Task<int> Update(int id, NoteUserDatabase noteUserDatabase)
    {
        string query = "update note_user set password_hash = $2, name = $3 " +
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
                    new NpgsqlParameter() { Value = noteUserDatabase.PasswordHash },
                    new NpgsqlParameter() { Value = noteUserDatabase.Name }
                }
            };

            return await command.ExecuteNonQueryAsync();
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

    public async Task<int> Update(string email, NoteUserDatabase noteUserDatabase)
    {
        string query = "update note_user set password_hash = $2, name = $3 " +
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
                    new NpgsqlParameter() { Value = noteUserDatabase.PasswordHash },
                    new NpgsqlParameter() { Value = noteUserDatabase.Name }
                }
            };

            return await command.ExecuteNonQueryAsync();
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
    
    public async Task<int> UpdatePassword(int id, string newPassword)
    {
        string query = "update note_user set password_hash = $2 " +
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

            return await command.ExecuteNonQueryAsync();
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

    public async Task<int> Delete(int id)
    {
        return await DeleteAsync("user", "id", id);
    }
}