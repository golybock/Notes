using Npgsql;

namespace NotesApi.Repositories;

public abstract class RepositoryBase
{
    private readonly IConfiguration _configuration;

    protected RepositoryBase(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private string? GetConnectionString()
    {
       return _configuration.GetConnectionString("notes");
    }
    
    protected NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(GetConnectionString());
    }

    protected async Task<int> DeleteAsync(string table, string column, object param)
    {
        var connection = GetConnection();

        try
        {
            connection.Open();

            var query = $"delete from {table} where {column} = $1";
            
            await using var cmd = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter {Value = param}
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
}