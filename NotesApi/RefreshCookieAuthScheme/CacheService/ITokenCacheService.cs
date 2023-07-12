using NotesApi.RefreshCookieAuthScheme.Token;

namespace NotesApi.RefreshCookieAuthScheme.CacheService;

public interface ITokenCacheService
{
    public Task<Tokens?> GetTokens(string email, string refreshToken);

    public Task SetTokens(string email, Tokens tokens, DateTime refreshTokenLifeTime);
}