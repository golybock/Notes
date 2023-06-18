using NotesApi.RefreshCookieAuthScheme.Token;

namespace NotesApi.RefreshCookieAuthScheme.Cookie;

public class CookieManager : CookieManagerBase, ICookieManager
{
    private readonly RefreshCookieOptions? _options;
    private readonly IConfiguration? _configuration;

    public CookieManager(RefreshCookieOptions options)
    {
        _options = options;
    }

    public CookieManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private CookieOptions DefaultOptions(DateTime expires) =>
        new (){Expires = expires, Secure = true, SameSite = SameSiteMode.None};

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

    public void SetTokens(HttpContext context, Tokens tokensDomain)
    {
        // validation lifetime from options
        int tokenValidityInDays = GetRefreshTokenLifeTime();
        var expires = DateTime.UtcNow.AddDays(tokenValidityInDays);

        // cookie expires and mode
        var options = DefaultOptions(expires);

        AppendResponseCookie(context, CookiesList.Token, tokensDomain.Token, options);
        AppendResponseCookie(context, CookiesList.RefreshToken, tokensDomain.RefreshToken, options);
    }
    
    public void SetTokens(HttpResponse response, Tokens tokensDomain)
    {
        // validation lifetime from options
        int tokenValidityInDays = GetRefreshTokenLifeTime();
        var expires = DateTime.UtcNow.AddDays(tokenValidityInDays);

        // cookie expires and mode
        var options = DefaultOptions(expires);

        AppendResponseCookie(response, CookiesList.Token, tokensDomain.Token, options);
        AppendResponseCookie(response, CookiesList.RefreshToken, tokensDomain.RefreshToken, options);
    }

    public void DeleteTokens(HttpContext context)
    {
        // validation lifetime from options
        int tokenValidityInDays = GetRefreshTokenLifeTime();
        var expires = DateTime.UtcNow.AddDays(tokenValidityInDays);

        // cookie expires
        var options = DefaultOptions(expires);
        
        DeleteCookie(context, CookiesList.Token, options);
        DeleteCookie(context, CookiesList.RefreshToken, options);
    }
    
    public void DeleteTokens(HttpResponse response)
    {
        // validation lifetime from options
        int tokenValidityInDays = GetRefreshTokenLifeTime();
        var expires = DateTime.UtcNow.AddDays(tokenValidityInDays);

        // cookie expires
        var options = DefaultOptions(expires);
        
        DeleteCookie(response, CookiesList.Token, options);
        DeleteCookie(response, CookiesList.RefreshToken, options);
    }

    private int GetRefreshTokenLifeTime()
    {
        if (_options == null)
        {
            if (_configuration == null)
                throw new Exception("Invalid configuration and options");
            
            return int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]!);
        }

        return _options.RefreshTokenLifeTimeInDays;
    }
}