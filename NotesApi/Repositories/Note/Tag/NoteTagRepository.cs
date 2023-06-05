using Database.Note.Tag;
using NotesApi.Repositories.Interfaces.Note.Tag;
using NotesApi.Repositories.Readers.Note.Tag;
using Npgsql;

namespace NotesApi.Repositories.Note.Tag;

public class NoteTagRepository : RepositoryBase, INoteTagRepository
{
    public NoteTagRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<NoteTagDatabase?> Get(int id)
    {
        string query = "select * from note_tag where id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = {new NpgsqlParameter() {Value = id}}
            };

            await using var reader = await command.ExecuteReaderAsync();

            // returns readed value(null if not found)
            return await NoteTagReader.ReadAsync(reader);
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

    public async Task<int> Create(NoteTagDatabase noteTagDatabase)
    {
        string query = "insert into note_tag(note_id, tag_id) values ($1, $2) returning id";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = noteTagDatabase.NoteId },
                    new NpgsqlParameter() { Value = noteTagDatabase.TagId }
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

    public async Task<bool> Delete(int id)
    {
        return await DeleteAsync("note_tag", "id", id) > 0;
    }

    public async Task<bool> DeleteByNote(Guid noteId)
    {
        return await DeleteAsync("note_tag", "note_id", noteId) > 0;
    }

    public async Task<bool> DeleteByTag(int tagId)
    {
        return await DeleteAsync("note_tag", "tag_id", tagId) > 0;
    }
}