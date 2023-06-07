using Domain.User;

namespace NotesApi.Auth.Cookie;

public class CookieManager : CookieManagerBase
{
    private readonly IConfiguration _configuration;
    private readonly HttpContext _context;

    public CookieManager(IConfiguration configuration, HttpContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public TokensDomain GetTokens()
    {
        string token = GetRequestCookie(_context, CookiesList.Token) ??
                       throw new InvalidOperationException("Token not found");

        string refreshToken = GetRequestCookie(_context, CookiesList.RefreshToken) ??
                              throw new InvalidOperationException("RefreshToken not found");

        var tokens = new TokensDomain()
        {
            Token = token,
            RefreshToken = refreshToken
        };

        return tokens;
    }

    public void SetTokens(TokensDomain tokensDomain)
    {
        // validation lifetime from appsettings
        int tokenValidityInMinutes = int.Parse(_configuration["JWT:TokenValidityInMinutes"]!);
        var expires = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes);

        // cookie expires
        var options = new CookieOptions()
        {
            Expires = expires
        };

        AppendResponseCookie(_context, CookiesList.Token, tokensDomain.Token, options);
        AppendResponseCookie(_context, CookiesList.RefreshToken, tokensDomain.RefreshToken, options);
    }

    public void DeleteTokens()
    {
        DeleteCookie(_context, CookiesList.Token);
        DeleteCookie(_context, CookiesList.RefreshToken);
    }
}