using Database.Note;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Repositories.Interfaces.Note;
using Repositories.Readers.Note;

namespace Repositories.Repositories.Note;

public class NoteTypeRepository : RepositoryBase, INoteTypeRepository
{
    public NoteTypeRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<NoteTypeDatabase?> Get(int id)
    {
        string query = "select * from note_type where id = $1";

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
            return await NoteTypeReader.ReadAsync(reader);
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

    public async Task<List<NoteTypeDatabase>> Get()
    {
        string query = "select * from note_type";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection);

            await using var reader = await command.ExecuteReaderAsync();
            
            return await NoteTypeReader.ReadListAsync(reader);
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
}