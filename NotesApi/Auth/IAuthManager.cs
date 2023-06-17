using System.Security.Claims;
using Domain.User;
using NotesApi.Auth.Token;
using ICookieManager = NotesApi.Auth.Cookie.ICookieManager;

namespace NotesApi.Auth;

public interface IAuthManager
{
    public ICookieManager CookieManager { get; protected set; }
    
    public ITokenManager TokenManager { get; protected set; }
    
    public Task SignInAsync(HttpContext context, UserDomain user);
    
    public Task SignInAsync(HttpResponse response, UserDomain user);

    public Task SignInAsync(HttpResponse response, ClaimsPrincipal principal);

    public void SignOut(HttpContext context);

    public void SignOut(HttpResponse response);
}