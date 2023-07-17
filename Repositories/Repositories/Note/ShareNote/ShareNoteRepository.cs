using Database.Note;
using Database.User;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Repositories.Readers.Note;
using Repositories.Readers.User;

namespace Repositories.Repositories.Note.ShareNote;

public class ShareNoteRepository : RepositoryBase, IShareNotesRepository
{
    public ShareNoteRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<List<SharedNoteDatabase>> Get(Guid noteId)
    {
        string query = "select * from shared_notes where note_id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = {new NpgsqlParameter() {Value = noteId}}
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
                Parameters = {new NpgsqlParameter() {Value = userId}}
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

    public async Task<List<UserDatabase>> GetSharedUsers(Guid noteId)
    {
        string query = "select u.email as email, " +
                       "u.id as id, " +
                       "u.password_hash as password_hash " +
                       "from shared_notes s join users u on u.id = s.user_id where s.note_id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = {new NpgsqlParameter() {Value = noteId}}
            };

            await using var reader = await command.ExecuteReaderAsync();

            // returns value(empty if not found)
            return await UserReader.ReadListAsync(reader);
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
                    new NpgsqlParameter() {Value = sharedNoteDatabase.NoteId},
                    new NpgsqlParameter() {Value = sharedNoteDatabase.UserId},
                    new NpgsqlParameter() {Value = sharedNoteDatabase.PermissionsLevelId},
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

    public async Task<bool> Update(Guid noteId, Guid userId, int permissionsLevel)
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
                    new NpgsqlParameter() {Value = noteId},
                    new NpgsqlParameter() {Value = userId},
                    new NpgsqlParameter() {Value = permissionsLevel}
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

    public async Task<bool> Delete(Guid noteId, Guid userId)
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

            return await cmd.ExecuteNonQueryAsync() > 0;
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

    public async Task<bool> DeleteNoteShare(Guid noteId)
    {
        return await DeleteAsync("shared_notes", "note_id", noteId) > 0;
    }
}