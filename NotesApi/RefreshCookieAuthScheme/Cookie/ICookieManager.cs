using NotesApi.RefreshCookieAuthScheme.Token;

namespace NotesApi.RefreshCookieAuthScheme.Cookie;

public interface ICookieManager
{
    public Tokens GetTokens(HttpContext context);

    public Tokens? GetTokens(HttpRequest request);

    public void SetTokens(HttpContext context, Tokens tokensDomain);

    public void SetTokens(HttpResponse response, Tokens tokensDomain);

    public void DeleteTokens(HttpContext context);

    public void DeleteTokens(HttpResponse response);
}