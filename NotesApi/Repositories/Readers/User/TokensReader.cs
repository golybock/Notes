using System.Net;
using Database.User;
using Npgsql;

namespace NotesApi.Repositories.Readers.User;

public class TokensReader : IReader<TokensDatabase>
{
    public static async Task<TokensDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            TokensDatabase tokensDatabase = new TokensDatabase();
            
            tokensDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
            tokensDatabase.UserId = reader.GetInt32(reader.GetOrdinal("user_id"));
            tokensDatabase.Token = reader.GetString(reader.GetOrdinal("token"));
            tokensDatabase.RefreshToken = reader.GetString(reader.GetOrdinal("refresh_token"));
            tokensDatabase.CreationDate = reader.GetDateTime(reader.GetOrdinal("creation_date"));
            tokensDatabase.Ip = (IPAddress) reader.GetValue(reader.GetOrdinal("ip"));
            tokensDatabase.Active = reader.GetBoolean(reader.GetOrdinal("active"));
            
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
            
            tokensDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
            tokensDatabase.UserId = reader.GetInt32(reader.GetOrdinal("user_id"));
            tokensDatabase.Token = reader.GetString(reader.GetOrdinal("token"));
            tokensDatabase.RefreshToken = reader.GetString(reader.GetOrdinal("refresh_token"));
            tokensDatabase.CreationDate = reader.GetDateTime(reader.GetOrdinal("creation_date"));
            tokensDatabase.Ip = (IPAddress) reader.GetValue(reader.GetOrdinal("ip"));
            tokensDatabase.Active = reader.GetBoolean(reader.GetOrdinal("active"));
            
            tokensDatabases.Add(tokensDatabase);
        }

        return tokensDatabases;
    }
}