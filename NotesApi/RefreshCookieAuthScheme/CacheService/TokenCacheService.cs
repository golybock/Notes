using System.Text.Json;
using Database.User;
using Microsoft.Extensions.Caching.Distributed;

namespace NotesApi.RefreshCookieAuthScheme.CacheService;

public class TokenCacheService : ITokenCacheService
{
    private readonly IDistributedCache _cache;
    
    /// <summary>
    /// Allows delete all user tokens
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="refreshToken"></param>
    /// <returns>key</returns>
    private string Key(Guid userId, string refreshToken) => $"{userId}:{refreshToken}";
    
    public TokenCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<TokensModel?> GetTokens(Guid userId, string refreshToken)
    {
        var key = Key(userId, refreshToken);
        
        var tokens = await _cache.GetStringAsync(key);

        if (tokens == null)
            return null;

        return JsonSerializer.Deserialize<TokensModel>(tokens);
    }

    public async Task SetTokens(Guid userId, TokensModel tokens, TimeSpan refreshTokenLifeTime)
    {
        var tokenLifeTime = DateTime.UtcNow.AddDays(refreshTokenLifeTime.TotalDays);
        
        // tokens pair can be deleted when refresh token expired
        var options = new DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = new DateTimeOffset(tokenLifeTime)
        };

        var key = Key(userId, tokens.RefreshToken);

        var value = JsonSerializer.Serialize(tokens);

        await _cache.SetStringAsync(key, value, options);
    }

    public async Task SetTokens(Guid userId, TokensModel tokens, int refreshTokenLifeTimeInDays)
    {
        var tokenLifeTime = DateTime.UtcNow.AddDays(refreshTokenLifeTimeInDays);
        
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