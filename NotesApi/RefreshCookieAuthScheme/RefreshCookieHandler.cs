using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using ISystemClock = Microsoft.AspNetCore.Authentication.ISystemClock;

namespace NotesApi.RefreshCookieAuthScheme;

public class RefreshCookieHandler : AuthenticationHandler<RefreshCookieOptions>
{
    private readonly AuthManager.AuthManager _authManager;
    
    public RefreshCookieHandler(
        IOptionsMonitor<RefreshCookieOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) 
        : base(options, logger, encoder, clock)
    {
        RefreshCookieOptions o = options.Get(RefreshCookieDefaults.AuthenticationScheme);
        
        _authManager = new AuthManager.AuthManager(o);
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
            

            var principal = _authManager.TokenManager.GetPrincipalFromExpiredToken(tokens.Token!);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            
            if (tokenActive)
                return AuthenticateResult.Success(ticket);
            
            await _authManager.SignInAsync(Response, principal);

            return AuthenticateResult.Success(ticket);
        }
        
        return AuthenticateResult.Fail("Invalid tokens");
    }
}