using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Extensions.Options;
using NotesApi.Auth.Tokens;
using Repositories.Repositories.User;
using CookieManager = NotesApi.Auth.Cookie.CookieManager;
using ISystemClock = Microsoft.AspNetCore.Authentication.ISystemClock;

namespace NotesApi.Auth;

public class AuthHandler : AuthenticationHandler<AuthSchemeOptions>
{
    private readonly CookieManager _cookieManager;
    private readonly TokenManager _tokenManager;
    
    private readonly TokensRepository _tokensRepository;
    private readonly UserRepository _userRepository;
    
    public AuthHandler(
        IOptionsMonitor<AuthSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) 
        : base(options, logger, encoder, clock)
    {
        _cookieManager = new CookieManager(options.CurrentValue);
        _tokenManager = new TokenManager(options.CurrentValue);

        var connectionString = options.CurrentValue.ConnectionString;
        
        _tokensRepository = new TokensRepository(connectionString);
        _userRepository = new UserRepository(connectionString);
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var tokens = _cookieManager.GetTokens(Request);

        // check cookies
        if (tokens == null)
        {
            return AuthenticateResult.Fail("Tokens Not Found.");
        }

        // validate without lifetime
        if (_tokenManager.TokenValid(tokens.Token!))
        {
            var tokenActive = _tokenManager.TokenActive(tokens.Token!);

            if (tokenActive)
            {
                var principal = _tokenManager.GetPrincipalFromToken(tokens.Token!);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            
            // refresh token, set cookie and return Success
        }
        
        return AuthenticateResult.Fail("Invalid tokens");
    }
}