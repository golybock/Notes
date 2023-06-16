using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Repositories.Repositories;

public abstract class RepositoryBase
{
    private readonly IConfiguration? _configuration;
    private readonly string? _connectionString;

    protected RepositoryBase(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected RepositoryBase(string connectionString)
    {
        _connectionString = connectionString;
    }

    private string? GetConnectionString()
    {
        if (_configuration == null)
            return _connectionString;

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
                    new NpgsqlParameter { Value = param }
                }
            };

            return await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return -1;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}