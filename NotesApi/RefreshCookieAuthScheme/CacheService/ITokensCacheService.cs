using NotesApi.RefreshCookieAuthScheme.Token;

namespace NotesApi.RefreshCookieAuthScheme.CacheService;

public interface ITokensCacheService
{
    public Task<Tokens?> GetTokens(string email, string refreshToken);

    public Task SetTokens(string email, Tokens tokens);
}