using Database.User;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Repositories.Repositories.Interfaces.User;
using Repositories.Repositories.Readers.User;

namespace Repositories.Repositories.User;

public class TokensRepository : RepositoryBase, ITokenRepository
{
    public TokensRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<List<TokensDatabase>> GetList(int userId)
    {
        string query = "select * from tokens where user_id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = {new NpgsqlParameter() {Value = userId}}
            };

            await using var reader = await command.ExecuteReaderAsync();

            return await TokensReader.ReadListAsync(reader);
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

    public async Task<TokensDatabase?> Get(int id)
    {
        string query = "select * from tokens where id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = {new NpgsqlParameter() {Value = id}}
            };

            await using var reader = await command.ExecuteReaderAsync();

            return await TokensReader.ReadAsync(reader);
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

    public async Task<TokensDatabase?> Get(string token, string refreshToken)
    {
        string query = "select * from tokens where token = $1 and refresh_token = $2";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() {Value = token},
                    new NpgsqlParameter() {Value = refreshToken}
                }
            };

            await using var reader = await command.ExecuteReaderAsync();

            return await TokensReader.ReadAsync(reader);
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

    public async Task<bool> SetNotActive(int id)
    {
        string query = "update tokens set active = false where id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() {Value = id},
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

    public async Task<int> Create(TokensDatabase tokensDatabase)
    {
        string query = "insert into tokens(user_id, token, refresh_token, ip)" +
                       "values ($1, $2, $3, $4) returning id";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() {Value = tokensDatabase.UserId},
                    new NpgsqlParameter() {Value = tokensDatabase.Token},
                    new NpgsqlParameter() {Value = tokensDatabase.RefreshToken},
                    new NpgsqlParameter() {NpgsqlValue = tokensDatabase.Ip == null ? DBNull.Value : tokensDatabase.Ip}
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
        return await DeleteAsync("tokens", "id", id) > 0;
    }
}