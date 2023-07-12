using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using NotesApi.RefreshCookieAuthScheme.Token;

namespace NotesApi.RefreshCookieAuthScheme.CacheService;

public class TokensCacheService : ITokensCacheService
{
    private readonly IDistributedCache _cache;

    private readonly DateTime _refreshTokenLifeTime;
    
    private string Key(string email, string refreshToken) => $"{email}:{refreshToken}";
    
    private DateTime RefreshTokenLifeTime =>
        DateTime.UtcNow.AddTicks(_refreshTokenLifeTime.Ticks);

    public TokensCacheService(IDistributedCache cache, DateTime refreshTokenLifeTime)
    {
        _cache = cache;

        _refreshTokenLifeTime = refreshTokenLifeTime;
    }

    public async Task<Tokens?> GetTokens(string email, string refreshToken)
    {
        var tokens = await _cache.GetStringAsync(Key(email, refreshToken));

        if (tokens == null)
            return null;

        return JsonSerializer.Deserialize<Tokens>(tokens);
    }

    public async Task SetTokens(string email, Tokens tokens)
    {
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = new DateTimeOffset(RefreshTokenLifeTime)
        };

        var key = Key(email, tokens.RefreshToken);

        var value = JsonSerializer.Serialize(tokens);

        await _cache.SetStringAsync(key, value, options);
    }
}