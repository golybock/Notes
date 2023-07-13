using Database.User;
using NotesApi.RefreshCookieAuthScheme.Token;

namespace NotesApi.RefreshCookieAuthScheme.CacheService;

public interface ITokenCacheService
{
    public Task<TokensDatabase?> GetTokens(Guid userId, string refreshToken);

    public Task SetTokens(Guid userId, TokensDatabase tokens, DateTime refreshTokenLifeTime);
    
    public Task DeleteTokens(Guid userId, string refreshToken);
}