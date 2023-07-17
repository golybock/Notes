using Database.Note;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Repositories.Readers.Note;

namespace Repositories.Repositories.Note.NoteImage;

public class NoteImageRepository: RepositoryBase, INoteImageRepository
{
    public NoteImageRepository(IConfiguration configuration) : base(configuration) { }

    public NoteImageRepository(string connectionString) : base(connectionString) { }

    public async Task<List<NoteImageDatabase>> GetImages(Guid noteId)
    {
        string query = "select * from note_images left join note n on n.id = note_images.note_id where note_id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter() { Value = noteId } }
            };

            await using var reader = await command.ExecuteReaderAsync();

            // returns value(null if not found)
            return await NoteImageReader.ReadListAsync(reader);
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

    public async Task<Guid> Create(NoteImageDatabase noteImageDatabase)
    {
        string query = "insert into note_images(id, note_id, x, y, width, height)" +
                       "values ($1, $2, $3, $4, $5, $6) returning id";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = noteImageDatabase.Id },
                    new NpgsqlParameter() { Value = noteImageDatabase.NoteId },
                    new NpgsqlParameter() { Value = noteImageDatabase.X },
                    new NpgsqlParameter() { Value = noteImageDatabase.Y },
                    new NpgsqlParameter() { Value = noteImageDatabase.Width },
                    new NpgsqlParameter() { Value = noteImageDatabase.Height },
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

    public async Task<bool> Update(Guid id, NoteImageDatabase noteImageDatabase)
    {
        string query = "update note_images set height = $2, width = $3, x = $4, y = $5 " +
                       "where id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = id},
                    new NpgsqlParameter() { Value = noteImageDatabase.Height },
                    new NpgsqlParameter() { Value = noteImageDatabase.Width },
                    new NpgsqlParameter() { Value = noteImageDatabase.X },
                    new NpgsqlParameter() { Value = noteImageDatabase.Y }
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
        return await DeleteAsync("note_images", "id", id) > 0;
    }

    public async Task<bool> Clear(Guid noteId)
    {
        return await DeleteAsync("note_images", "note_id", noteId) > 0;
    }
}