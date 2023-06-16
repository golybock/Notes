namespace NotesApi.Auth.Cookie;

public class CookieManager : CookieManagerBase
{
    private readonly AuthSchemeOptions? _options;
    private readonly IConfiguration? _configuration;

    public CookieManager(AuthSchemeOptions options)
    {
        _options = options;
    }

    public CookieManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Blank.User.Tokens GetTokens(HttpContext context)
    {
        string? token = GetRequestCookie(context, CookiesList.Token);

        string? refreshToken = GetRequestCookie(context, CookiesList.RefreshToken);

        var tokens = new Blank.User.Tokens()
        {
            Token = token,
            RefreshToken = refreshToken
        };

        return tokens;
    }
    
    public Blank.User.Tokens? GetTokens(HttpRequest request)
    {
        string? token = GetRequestCookie(request, CookiesList.Token);

        string? refreshToken = GetRequestCookie(request, CookiesList.RefreshToken);

        if (token == null || refreshToken == null)
            return null;
        
        var tokens = new Blank.User.Tokens()
        {
            Token = token,
            RefreshToken = refreshToken
        };

        return tokens;
    }

    public void SetTokens(HttpContext context, Blank.User.Tokens tokensDomain)
    {
        // validation lifetime from options
        int tokenValidityInDays = GetRefreshTokenLifeTime();
        var expires = DateTime.UtcNow.AddDays(tokenValidityInDays);

        // cookie expires and mode
        var options = new CookieOptions()
        {
            Expires = expires,
            Secure = true,
            SameSite = SameSiteMode.None
        };

        AppendResponseCookie(context, CookiesList.Token, tokensDomain.Token, options);
        AppendResponseCookie(context, CookiesList.RefreshToken, tokensDomain.RefreshToken, options);
    }
    
    public void SetTokens(HttpResponse response, Blank.User.Tokens tokensDomain)
    {
        // validation lifetime from options
        int tokenValidityInDays = GetRefreshTokenLifeTime();
        var expires = DateTime.UtcNow.AddDays(tokenValidityInDays);

        // cookie expires and mode
        var options = new CookieOptions()
        {
            Expires = expires,
            Secure = true,
            SameSite = SameSiteMode.None
        };

        AppendResponseCookie(response, CookiesList.Token, tokensDomain.Token, options);
        AppendResponseCookie(response, CookiesList.RefreshToken, tokensDomain.RefreshToken, options);
    }

    public void DeleteTokens(HttpContext context)
    {
        // validation lifetime from options
        int tokenValidityInDays = GetRefreshTokenLifeTime();
        var expires = DateTime.UtcNow.AddDays(tokenValidityInDays);

        // cookie expires
        var options = new CookieOptions()
        {
            Expires = expires,
            Secure = true,
            SameSite = SameSiteMode.None
        };
        
        DeleteCookie(context, CookiesList.Token, options);
        DeleteCookie(context, CookiesList.RefreshToken, options);
    }
    
    public void DeleteTokens(HttpResponse response)
    {
        // validation lifetime from options
        int tokenValidityInDays = GetRefreshTokenLifeTime();
        var expires = DateTime.UtcNow.AddDays(tokenValidityInDays);

        // cookie expires
        var options = new CookieOptions()
        {
            Expires = expires,
            Secure = true,
            SameSite = SameSiteMode.None
        };
        
        DeleteCookie(response, CookiesList.Token, options);
        DeleteCookie(response, CookiesList.RefreshToken, options);
    }

    private int GetRefreshTokenLifeTime()
    {
        if (_options == null)
        {
            if (_configuration == null)
                throw new Exception("Ivalid configuration and options");
            
            return int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]!);
        }

        return _options.RefreshTokenLifeTimeInDays;
    }
}