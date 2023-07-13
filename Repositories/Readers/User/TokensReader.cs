using System.Net;
using Database.User;
using Npgsql;

namespace Repositories.Readers.User;

public class TokensReader : IReader<TokensDatabase>
{
    private const string Id = "id";
    private const string UserId = "user_id";
    private const string Token = "token";
    private const string RefreshToken = "refresh_token";
    private const string CreatedDate = "created_date";
    private const string Ip = "ip";

    public static async Task<TokensDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            TokensDatabase tokensDatabase = new TokensDatabase();
            
            tokensDatabase.Id = reader.GetInt32(reader.GetOrdinal(Id));
            tokensDatabase.UserId = reader.GetGuid(reader.GetOrdinal(UserId));
            tokensDatabase.Token = reader.GetString(reader.GetOrdinal(Token));
            tokensDatabase.RefreshToken = reader.GetString(reader.GetOrdinal(RefreshToken));
            tokensDatabase.CreationDate = reader.GetDateTime(reader.GetOrdinal(CreatedDate));
            tokensDatabase.Ip = reader.GetString(reader.GetOrdinal(Ip));

            return tokensDatabase;
        }

        return null;
    }

    public static async Task<List<TokensDatabase>> ReadListAsync(NpgsqlDataReader reader)
    {
        List<TokensDatabase> tokensDatabases = new List<TokensDatabase>();

        while (await reader.ReadAsync())
        {
            TokensDatabase tokensDatabase = new TokensDatabase();
            
            tokensDatabase.Id = reader.GetInt32(reader.GetOrdinal(Id));
            tokensDatabase.UserId = reader.GetGuid(reader.GetOrdinal(UserId));
            tokensDatabase.Token = reader.GetString(reader.GetOrdinal(Token));
            tokensDatabase.RefreshToken = reader.GetString(reader.GetOrdinal(RefreshToken));
            tokensDatabase.CreationDate = reader.GetDateTime(reader.GetOrdinal(CreatedDate));
            tokensDatabase.Ip = reader.GetString(reader.GetOrdinal(Ip));

            tokensDatabases.Add(tokensDatabase);
        }

        return tokensDatabases;
    }
}