using Database.Note;
using NotesApi.Repositories.Interfaces.Note;
using NotesApi.Repositories.Readers.Note;
using Npgsql;

namespace NotesApi.Repositories.Note;

public class ShareNoteRepository : RepositoryBase, ISharedNotesRepository
{
    public ShareNoteRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<List<SharedNoteDatabase>> Get(Guid noteId)
    {
        string query = "select * from shared_notes where note_id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter() { Value = noteId } }
            };

            await using var reader = await command.ExecuteReaderAsync();

            // returns value(empty if not found)
            return await SharedNoteReader.ReadListAsync(reader);
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

    public async Task<List<SharedNoteDatabase>> Get(int userId)
    {
        string query = "select * from shared_notes where user_id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter() { Value = userId } }
            };

            await using var reader = await command.ExecuteReaderAsync();

            // returns value(empty if not found)
            return await SharedNoteReader.ReadListAsync(reader);
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

    public async Task<int> Create(SharedNoteDatabase sharedNoteDatabase)
    {
        string query = "insert into shared_notes(note_id, user_id, permissions_level_id)" +
                       "values ($1, $2, $3) returning id";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = sharedNoteDatabase.NoteId },
                    new NpgsqlParameter() { Value = sharedNoteDatabase.UserId },
                    new NpgsqlParameter() { Value = sharedNoteDatabase.PermissionsLevelId },
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

    public async Task<int> Update(Guid noteId, int userId, int permissionsLevel)
    {
        string query = "update shared_notes set permissions_level_id = $3 where note_id = $1 and user_id = $2";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = noteId},
                    new NpgsqlParameter() { Value = userId},
                    new NpgsqlParameter() { Value = permissionsLevel }
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

    public async Task<int> Delete(Guid noteId, int userId)
    {
        var connection = GetConnection();

        try
        {
            connection.Open();

            var query = $"delete from shared_notes where note_id = $1 and user_id = $2";
            
            await using var cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = noteId},
                    new NpgsqlParameter {Value = userId}
                }
            };

            return await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            return -1;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<int> DeleteNote(Guid noteId)
    {
        return await DeleteAsync("shared_notes", "note_id", noteId);
    }
}