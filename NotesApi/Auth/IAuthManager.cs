using System.Security.Claims;
using Domain.User;
using NotesApi.Auth.Cookie;
using NotesApi.Auth.Tokens;

namespace NotesApi.Auth;

public interface IAuthManager
{
    public CookieManager CookieManager { get; protected set; }
    
    public TokenManager TokenManager { get; protected set; }
    
    public Task SignInAsync(HttpContext context, UserDomain user);
    
    public Task SignInAsync(HttpResponse response, UserDomain user);
    
    public Task SignInAsync(HttpContext context, ClaimsPrincipal claimsPrincipal);
    
    public Task SignInAsync(HttpResponse response, ClaimsPrincipal claimsPrincipal);

    public void SignOut(HttpContext context);

    public void SignOut(HttpResponse response);
}