using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Database.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Repositories.Repositories.User;
using CookieManager = NotesApi.Auth.Cookie.CookieManager;
using ISystemClock = Microsoft.AspNetCore.Authentication.ISystemClock;

namespace NotesApi.Auth;

public class AuthHandler : AuthenticationHandler<AuthSchemeOptions>
{
    private AuthManager _authManager;
    
    public AuthHandler(
        IOptionsMonitor<AuthSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) 
        : base(options, logger, encoder, clock)
    {
        _authManager = new AuthManager(options.CurrentValue);
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var tokens = _authManager.CookieManager.GetTokens(Request);

        // check cookies
        if (tokens == null)
        {
            return AuthenticateResult.Fail("Tokens Not Found.");
        }

        // validate without lifetime
        if (_authManager.TokenManager.TokenValid(tokens.Token!))
        {
            var tokenActive = _authManager.TokenManager.TokenActive(tokens.Token!);

            var principal = _authManager.TokenManager.GetPrincipalFromToken(tokens.Token!);
            
            if (tokenActive)
            {
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }


            await _authManager.SignInAsync(Response, principal);

            // refresh token, set cookie and return Success
        }
        
        return AuthenticateResult.Fail("Invalid tokens");
    }
}