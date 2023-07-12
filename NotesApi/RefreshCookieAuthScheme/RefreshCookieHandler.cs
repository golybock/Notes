﻿using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using NotesApi.RefreshCookieAuthScheme.AuthManager;
using NotesApi.RefreshCookieAuthScheme.CacheService;
using ISystemClock = Microsoft.AspNetCore.Authentication.ISystemClock;

namespace NotesApi.RefreshCookieAuthScheme;

public class RefreshCookieHandler : AuthenticationHandler<RefreshCookieOptions>
{
    private readonly IAuthManager _authManager;
    private readonly ITokensCacheService _tokensCacheService;
    
    public RefreshCookieHandler(
        IOptionsMonitor<RefreshCookieOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) 
        : base(options, logger, encoder, clock)
    {
        var o = options.Get(RefreshCookieDefaults.AuthenticationScheme);
        
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

            try
            {
                await _authManager.RefreshTokensAsync(Response, principal, tokens);
                
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception e)
            {
                return AuthenticateResult.Fail("Refresh token not active");
            }
        }
        
        return AuthenticateResult.Fail("Invalid tokens");
    }
}