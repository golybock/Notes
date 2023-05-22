using Database.Note.Tag;
using NotesApi.Repositories.Interfaces;
using NotesApi.Repositories.Interfaces.Note.Tag;
using Npgsql;

namespace NotesApi.Repositories.Note.Tag;

public class TagRepository : RepositoryBase, ITagRepository
{
    public TagRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<TagDatabase?> Get(int id)
    {
        TagDatabase tagDatabase = new TagDatabase();

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

            while (await reader.ReadAsync())
            {
                tagDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
                tagDatabase.Name = reader.GetString(reader.GetOrdinal("name"));
                return tagDatabase;
            }

            return null;
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
        List<TagDatabase> tagDatabases = new List<TagDatabase>();

        string query = "select * from tag";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection);

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                TagDatabase tagDatabase = new TagDatabase();

                tagDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
                tagDatabase.Name = reader.GetString(reader.GetOrdinal("name"));

                tagDatabases.Add(tagDatabase);
            }

            return tagDatabases;
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

    public async Task<List<TagDatabase>> GetNoteTags(int noteId)
    {
        List<TagDatabase> tagDatabases = new List<TagDatabase>();

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

            while (await reader.ReadAsync())
            {
                TagDatabase tagDatabase = new TagDatabase();

                tagDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
                tagDatabase.Name = reader.GetString(reader.GetOrdinal("name"));

                tagDatabases.Add(tagDatabase);
            }

            return tagDatabases;
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
        string query = "insert into tag(name) values ($1) returning id";

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

    public async Task<int> Update(int id, TagDatabase tagDatabase)
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
        return await DeleteAsync("tag", "id", id);
    }
}