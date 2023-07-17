using Database.User;
using NotesApi.RefreshCookieAuthScheme.Token;

namespace NotesApi.RefreshCookieAuthScheme.CacheService;

public interface ITokenCacheService
{
    public Task<TokensModel?> GetTokens(Guid userId, string refreshToken);

    public Task SetTokens(Guid userId, TokensModel tokens, TimeSpan refreshTokenLifeTime);
    
    public Task SetTokens(Guid userId, TokensModel tokens, int refreshTokenLifeTimeInDays);
    
    public Task DeleteTokens(Guid userId, string refreshToken);
}