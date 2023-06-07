using Microsoft.AspNetCore.Authentication.Cookies;

namespace NotesApi.Auth.Cookie;

// base realize cookie-manager
public class CookieManagerBase : ICookieManager
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