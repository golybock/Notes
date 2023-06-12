using Blank.User;
using Domain.User;

namespace NotesApi.Auth.Cookie;

public class CookieManager : CookieManagerBase
{
    private readonly IConfiguration _configuration;
    // private readonly HttpContext _context;

    public CookieManager(IConfiguration configuration)
    {
        _configuration = configuration;
        // _context = context;
    }

    public TokensBlank GetTokens(HttpContext context)
    {
        string token = GetRequestCookie(context, CookiesList.Token) ??
                       throw new InvalidOperationException("Token not found");

        string refreshToken = GetRequestCookie(context, CookiesList.RefreshToken) ??
                              throw new InvalidOperationException("RefreshToken not found");

        var tokens = new TokensBlank()
        {
            Token = token,
            RefreshToken = refreshToken
        };

        return tokens;
    }

    public void SetTokens(HttpContext context, TokensBlank tokensDomain)
    {
        // validation lifetime from appsettings
        int tokenValidityInDays = int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]!);
        var expires = DateTime.UtcNow.AddDays(tokenValidityInDays);

        // cookie expires
        var options = new CookieOptions()
        {
            Expires = expires,
            Secure = true,
            SameSite = SameSiteMode.None
        };

        AppendResponseCookie(context, CookiesList.Token, tokensDomain.Token, options);
        AppendResponseCookie(context, CookiesList.RefreshToken, tokensDomain.RefreshToken, options);
    }

    public void DeleteTokens(HttpContext context)
    {
        // validation lifetime from appsettings
        int tokenValidityInDays = int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]!);
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
}