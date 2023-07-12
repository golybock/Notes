using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using NotesApi.RefreshCookieAuthScheme.Token;

namespace NotesApi.RefreshCookieAuthScheme.CacheService;

public class TokenCacheService : ITokenCacheService
{
    private readonly IDistributedCache _cache;

    private string Key(string email, string refreshToken) => $"{email}:{refreshToken}";

    public TokenCacheService(IDistributedCache cache)
    {
        _cache = cache;
        
    }

    public async Task<Tokens?> GetTokens(string email, string refreshToken)
    {
        var tokens = await _cache.GetStringAsync(Key(email, refreshToken));

        if (tokens == null)
            return null;

        return JsonSerializer.Deserialize<Tokens>(tokens);
    }

    public async Task SetTokens(string email, Tokens tokens, DateTime refreshTokenLifeTime)
    {
        var tokenLifeTime = DateTime.UtcNow.AddTicks(refreshTokenLifeTime.Ticks);
        
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = new DateTimeOffset(tokenLifeTime)
        };

        var key = Key(email, tokens.RefreshToken);

        var value = JsonSerializer.Serialize(tokens);

        await _cache.SetStringAsync(key, value, options);
    }
}