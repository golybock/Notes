using Database.Note;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Repositories.Repositories.Interfaces.Note;
using Repositories.Repositories.Readers.Note;

namespace Repositories.Repositories.Note;

public class NoteRepository : RepositoryBase, INoteRepository
{
    public NoteRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<NoteDatabase?> GetNote(Guid guid)
    {
        string query = "select * from note where id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter() { Value = guid } }
            };

            await using var reader = await command.ExecuteReaderAsync();

            // returns value(null if not found)
            return await NoteReader.ReadAsync(reader);
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

    public async Task<List<NoteDatabase>> GetNotes(Guid userId)
    {
        string query = "select * from note where owner_id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter() { Value = userId} }
            };

            await using var reader = await command.ExecuteReaderAsync();
            
            return await NoteReader.ReadListAsync(reader);
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

    public async Task<List<NoteDatabase>> GetShared(Guid userId)
    {
        string query = "select * from note join shared_notes sn on note.id = sn.note_id where sn.user_id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter() { Value = userId} }
            };

            await using var reader = await command.ExecuteReaderAsync();
            
            return await NoteReader.ReadListAsync(reader);
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

    public async Task<Guid> Create(NoteDatabase noteDatabase)
    {
        string query = "insert into note(header, created_date, edited_date, source_path, owner_id, id, type_id)" +
                       "values ($1, $2, $3, $4, $5, $6, $7) returning id";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = noteDatabase.Header },
                    new NpgsqlParameter() { Value = noteDatabase.CreationDate },
                    new NpgsqlParameter() { Value = noteDatabase.EditedDate },
                    new NpgsqlParameter() { Value = noteDatabase.SourcePath == null ? DBNull.Value : noteDatabase.SourcePath },
                    new NpgsqlParameter() { Value = noteDatabase.OwnerId },
                    new NpgsqlParameter() { Value = noteDatabase.Id },
                    new NpgsqlParameter() { Value = 1 },
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
    
    public async Task<bool> Update(Guid guid, NoteDatabase noteDatabase)
    {
        string query = "update note set header = $2, edited_date = $3 " +
                       "where id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = guid},
                    new NpgsqlParameter() { Value = noteDatabase.Header },
                    new NpgsqlParameter() { Value = noteDatabase.EditedDate }
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

    public async Task<bool> UpdateType(Guid id, int type)
    {
        string query = "update note set type_id = $2 where id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = id},
                    new NpgsqlParameter() { Value = type }
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
        return await DeleteAsync("note", "id", id) > 0;
    }
}