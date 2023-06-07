using Domain.User;
using ICookieManager = Microsoft.AspNetCore.Authentication.Cookies.ICookieManager;

namespace NotesApi.Auth.Cookie;

public class CookieManager
{
    private readonly CookieManagerBackend _cookieManagerBackend;
    private readonly IConfiguration _configuration;

    public CookieManager(IConfiguration configuration)
    {
        _configuration = configuration;
        
        _cookieManagerBackend = new CookieManagerBackend();
    }

    public TokensDomain GetTokens(HttpContext context)
    {
        string token = _cookieManagerBackend.GetRequestCookie(context, "token") ??
                       throw new InvalidOperationException("Token not found");

        string refreshToken = _cookieManagerBackend.GetRequestCookie(context, "refreshToken") ??
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
        int tokenValidityInMinutes = int.Parse(_configuration["JWT:TokenValidityInMinutes"]!);
        var expires = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes);

        var options = new CookieOptions()
        {
            Expires = expires
        };
        
        _cookieManagerBackend.AppendResponseCookie(context, CookiesList.Token, tokensDomain.Token, options);
        _cookieManagerBackend.AppendResponseCookie(context, CookiesList.RefreshToken, tokensDomain.RefreshToken, options);
    }

    public void DeleteTokens(HttpContext context)
    {
        _cookieManagerBackend.DeleteCookie(context, "token");
        _cookieManagerBackend.DeleteCookie(context, "refreshToken");
    }
}

internal class CookieManagerBackend : ICookieManager
{
    public string? GetRequestCookie(HttpContext context, string key)
    {
        return context.Request.Cookies.FirstOrDefault(c => c.Key == key).Value;
    }

    public void AppendResponseCookie(HttpContext context, string key, string? value, CookieOptions options)
    {
        context.Response.Cookies.Append(key, value!, options);
    }

    public void DeleteCookie(HttpContext context, string key)
    {
        context.Response.Cookies.Delete(key);
    }
    
    public void DeleteCookie(HttpContext context, string key, CookieOptions options)
    {
        context.Response.Cookies.Delete(key, options);
    }
}