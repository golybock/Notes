using Database.Note;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Repositories.Readers.Note;

namespace Repositories.Repositories.Note;

public class NoteRepository : RepositoryBase, INoteRepository
{
    public NoteRepository(IConfiguration configuration) : base(configuration) { }

    // Get note if available for user
    public async Task<NoteDatabase?> GetNote(Guid guid, Guid userId)
    {
        string query = "select * from note left join shared_notes sn on note.id = sn.note_id " +
                       "where note.id = $1::uuid and " +
                       "(note.owner_id = $2::uuid or sn.user_id = $2::uuid) " +
                       "limit 1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = guid },
                    new NpgsqlParameter() { Value = userId }
                }
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
        string query = "select * from note left join shared_notes " +
                       "sn on note.id = sn.note_id where sn.user_id = $1";

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
        string query = "insert into note(header, owner_id, id, type_id)" +
                       "values ($1, $2, $3, $4) returning id";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = noteDatabase.Header },
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

    public async Task<NoteDatabase?> GetSharedNote(Guid userId, Guid noteId)
    {
        string query =
            "select * from note n " +
            "inner join shared_notes sn " +
            "on n.id = sn.note_id " +
            "where sn.user_id = $1 and " +
            "sn.note_id = $2 ";
        
        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = userId },
                    new NpgsqlParameter() { Value = noteId }
                }
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

    public async Task<bool> Delete(Guid id)
    {
        return await DeleteCascadeAsync("note", "id", id) > 0;
    }
}