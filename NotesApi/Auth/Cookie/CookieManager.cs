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

    public TokensDomain GetTokens(HttpContext context)
    {
        string token = GetRequestCookie(context, CookiesList.Token) ??
                       throw new InvalidOperationException("Token not found");

        string refreshToken = GetRequestCookie(context, CookiesList.RefreshToken) ??
                              throw new InvalidOperationException("RefreshToken not found");

        var tokens = new TokensDomain()
        {
            Token = token,
            RefreshToken = refreshToken
        };

        return tokens;
    }

    public void SetTokens(HttpContext context, TokensDomain tokensDomain)
    {
        // validation lifetime from appsettings
        int tokenValidityInMinutes = int.Parse(_configuration["JWT:TokenValidityInMinutes"]!);
        var expires = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes);

        // cookie expires
        var options = new CookieOptions()
        {
            Expires = expires
        };

        AppendResponseCookie(context, CookiesList.Token, tokensDomain.Token, options);
        AppendResponseCookie(context, CookiesList.RefreshToken, tokensDomain.RefreshToken, options);
    }

    public void DeleteTokens(HttpContext context)
    {
        DeleteCookie(context, CookiesList.Token);
        DeleteCookie(context, CookiesList.RefreshToken);
    }
}