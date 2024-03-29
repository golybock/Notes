using NotesApi.RefreshCookieAuthScheme.Token;

namespace NotesApi.RefreshCookieAuthScheme.Cookie;

// todo maybe delete code duplicate lines (if can be needed)
public class CookieManager : CookieManagerBase, ICookieManager
{
    private CookieOptions DefaultOptions(DateTime expires) =>
        new (){Expires = expires, Secure = true, SameSite = SameSiteMode.None};
    
    private CookieOptions DefaultOptions() =>
        new (){Secure = true, SameSite = SameSiteMode.None};

    public Tokens? GetTokens(HttpContext context)
    {
        string? token = GetRequestCookie(context, CookiesList.Token);

        string? refreshToken = GetRequestCookie(context, CookiesList.RefreshToken);

        if (token == null || refreshToken == null)
            return null;
        
        var tokens = new Tokens()
        {
            Token = token,
            RefreshToken = refreshToken
        };

        return tokens;
    }
    
    public Tokens? GetTokens(HttpRequest request)
    {
        string? token = GetRequestCookie(request, CookiesList.Token);

        string? refreshToken = GetRequestCookie(request, CookiesList.RefreshToken);

        if (token == null || refreshToken == null)
            return null;
        
        var tokens = new Tokens()
        {
            Token = token,
            RefreshToken = refreshToken
        };

        return tokens;
    }

    public void SetTokens(HttpContext context, Tokens tokensDomain, int refreshTokenLifeTimeInDays)
    {
        // validation lifetime from options
        var expires = DateTime.UtcNow.AddDays(refreshTokenLifeTimeInDays);

        // cookie expires and mode
        var options = DefaultOptions(expires);

        AppendResponseCookie(context, CookiesList.Token, tokensDomain.Token, options);
        AppendResponseCookie(context, CookiesList.RefreshToken, tokensDomain.RefreshToken, options);
    }
    
    public void SetTokens(HttpResponse response, Tokens tokensDomain, int refreshTokenLifeTimeInDays)
    {
        // validation lifetime from options
        var expires = DateTime.UtcNow.AddDays(refreshTokenLifeTimeInDays);

        // cookie expires and mode
        var options = DefaultOptions(expires);

        AppendResponseCookie(response, CookiesList.Token, tokensDomain.Token, options);
        AppendResponseCookie(response, CookiesList.RefreshToken, tokensDomain.RefreshToken, options);
    }

    public void DeleteTokens(HttpContext context)
    {
        var options = DefaultOptions();
        
        DeleteCookie(context, CookiesList.Token, options);
        DeleteCookie(context, CookiesList.RefreshToken, options);
    }
    
    public void DeleteTokens(HttpResponse response)
    {
        var options = DefaultOptions();
        
        DeleteCookie(response, CookiesList.Token, options);
        DeleteCookie(response, CookiesList.RefreshToken, options);
    }
}