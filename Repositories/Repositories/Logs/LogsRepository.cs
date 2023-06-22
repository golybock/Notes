using Database.Logs;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Repositories.Interfaces.Logs;
using Repositories.Readers.Logs;

namespace Repositories.Repositories.Logs;

public class LogsRepository : RepositoryBase, ILogRepository
{
    public LogsRepository(IConfiguration configuration) : base(configuration) { }

    public LogsRepository(string connectionString) : base(connectionString) { }

    public async Task<LogsDatabase?> Get(int id)
    {
        string query = "select * from logs where id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter() { Value = id } }
            };

            await using var reader = await command.ExecuteReaderAsync();

            // returns value(null if not found)
            return await LogsReader.ReadAsync(reader);
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

    public async Task<List<LogsDatabase>> GetUser(Guid userId)
    {
        string query = "select * from logs where user_id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter() { Value = userId} }
            };

            await using var reader = await command.ExecuteReaderAsync();
            
            return await LogsReader.ReadListAsync(reader);
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

    public async Task<List<LogsDatabase>> GetNote(Guid noteId)
    {
        string query = "select * from logs where note_id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter() { Value = noteId} }
            };

            await using var reader = await command.ExecuteReaderAsync();
            
            return await LogsReader.ReadListAsync(reader);
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

    public async Task<int> Create(LogsDatabase noteDatabase)
    {
        string query = "insert into logs(action, user_id, note_id)" +
                       "values ($1, $2, $3) returning id";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = noteDatabase.Action },
                    new NpgsqlParameter() { Value = noteDatabase.UserId },
                    new NpgsqlParameter() { Value = noteDatabase.NoteId == null ? DBNull.Value : noteDatabase.NoteId }
                }
            };

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                return reader.GetInt32(reader.GetOrdinal("id"));

            return 0;
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
        return await DeleteAsync("logs", "id", id);
    }

    public async Task<int> DeleteUser(Guid userId)
    {
        return await DeleteAsync("logs", "user_id", userId);
    }

    public async Task<int> DeleteNotes(Guid noteId)
    {
        return await DeleteAsync("logs", "note_id", noteId);
    }
}