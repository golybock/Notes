using Database.Note;
using NotesApi.Repositories.Interfaces.Note;
using NotesApi.Repositories.Readers.Note;
using Npgsql;

namespace NotesApi.Repositories.Note;

public class PermissionsLevelRepository : RepositoryBase, IPermissionsLevelRepository
{
    public PermissionsLevelRepository(IConfiguration configuration) : base(configuration) { }
    
    public async Task<PermissionsLevelDatabase?> Get(int id)
    {
        string query = "select * from permissions_level where id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter() { Value = id} }
            };

            await using var reader = await command.ExecuteReaderAsync();
            
            return await PermissionsLevelReader.ReadAsync(reader);
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

    public async Task<List<PermissionsLevelDatabase>> Get()
    {
        string query = "select * from permissions_level";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection);

            await using var reader = await command.ExecuteReaderAsync();
            
            return await PermissionsLevelReader.ReadListAsync(reader);
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