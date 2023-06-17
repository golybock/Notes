namespace NotesApi.RefreshCookieAuthScheme.Cookie;

// base realize cookie-manager
public class CookieManagerBase : Microsoft.AspNetCore.Authentication.Cookies.ICookieManager
{
    public string? GetRequestCookie(HttpContext context, string key)
    {
        return context.Request.Cookies.FirstOrDefault(c => c.Key == key).Value;
    }

    protected string? GetRequestCookie(HttpRequest request, string key)
    {
        return request.Cookies.FirstOrDefault(c => c.Key == key).Value;
    }

    public void AppendResponseCookie(HttpContext context, string key, string? value, CookieOptions options)
    {
        context.Response.Cookies.Append(key, value!, options);
    }
    
    protected void AppendResponseCookie(HttpResponse response, string key, string? value, CookieOptions options)
    {
        response.Cookies.Append(key, value!, options);
    }

    public void DeleteCookie(HttpContext context, string key, CookieOptions options)
    {
        context.Response.Cookies.Delete(key, options);
    }
    
    protected void DeleteCookie(HttpResponse response, string key, CookieOptions options)
    {
        response.Cookies.Delete(key, options);
    }
}