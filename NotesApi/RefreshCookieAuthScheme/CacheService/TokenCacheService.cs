using System.Text.Json;
using Database.User;
using Microsoft.Extensions.Caching.Distributed;

namespace NotesApi.RefreshCookieAuthScheme.CacheService;

public class TokenCacheService : ITokenCacheService
{
    private readonly IDistributedCache _cache;

    private readonly RefreshCookieOptions _options;

    private string Key(Guid userId, string refreshToken) => $"{userId}:{refreshToken}";
    
    public TokenCacheService(IDistributedCache cache, RefreshCookieOptions options)
    {
        _cache = cache;
        _options = options;
    }

    public async Task<TokensDatabase?> GetTokens(Guid userId, string refreshToken)
    {
        var key = Key(userId, refreshToken);
        
        var tokens = await _cache.GetStringAsync(key);

        if (tokens == null)
            return null;

        return JsonSerializer.Deserialize<TokensDatabase>(tokens);
    }

    public async Task SetTokens(Guid userId, TokensDatabase tokens, TimeSpan refreshTokenLifeTime)
    {
        var tokenLifeTime = DateTime.UtcNow.AddDays(_options.RefreshTokenLifeTimeInDays);
        
        // tokens pair can be deleted when refresh token expired
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = new DateTimeOffset(tokenLifeTime)
        };

        var key = Key(userId, tokens.RefreshToken);

        var value = JsonSerializer.Serialize(tokens);

        await _cache.SetStringAsync(key, value, options);
    }

    public async Task DeleteTokens(Guid userId, string refreshToken)
    {
        var key = Key(userId, refreshToken);

        await _cache.RemoveAsync(key);
    }
}