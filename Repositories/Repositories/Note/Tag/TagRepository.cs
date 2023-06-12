using Database.Note.Tag;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Repositories.Repositories.Interfaces.Note.Tag;
using Repositories.Repositories.Readers.Note.Tag;

namespace Repositories.Repositories.Note.Tag;

public class TagRepository : RepositoryBase, ITagRepository
{
    public TagRepository(IConfiguration configuration) : base(configuration) { }
    
    public async Task<TagDatabase?> Get(Guid id)
    {
        string query = "select * from tag where id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter() { Value = id } }
            };

            await using var reader = await command.ExecuteReaderAsync();

            // returns readed values(null if not found)
            return await TagReader.ReadAsync(reader);
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

    public async Task<List<TagDatabase>> Get()
    {
        string query = "select * from tag";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection);

            await using var reader = await command.ExecuteReaderAsync();

            // returns readed values
            return await TagReader.ReadListAsync(reader);
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

    public async Task<List<TagDatabase>> GetNoteTags(Guid noteId)
    {
        string query = "select * from tag join note_tag nt on tag.id = nt.tag_id where nt.note_id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter() { Value = noteId } }
            };

            await using var reader = await command.ExecuteReaderAsync();
            
            return await TagReader.ReadListAsync(reader);
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

    public async Task<int> Create(TagDatabase tagDatabase)
    {
        string query = "insert into tag(id, name) values ($1) returning id";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter() { Value = tagDatabase.Name } }
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

    public async Task<bool> DeleteNoteTags(Guid noteId)
    {
        return await DeleteAsync("note_tag", "note_id", noteId) > 0;
    }

    public async Task<bool> Update(int id, TagDatabase tagDatabase)
    {
        string query = "update tag set name = $2 where id = $1 ";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = id },
                    new NpgsqlParameter() { Value = tagDatabase.Name }
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

    public async Task<bool> Delete(int id)
    {
        return await DeleteAsync("tag", "id", id) > 0;
    }
}