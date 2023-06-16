using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Extensions.Options;
using NotesApi.Auth.Tokens;
using CookieManager = NotesApi.Auth.Cookie.CookieManager;
using ISystemClock = Microsoft.AspNetCore.Authentication.ISystemClock;

namespace NotesApi.Auth;

public class AuthHandler : AuthenticationHandler<AuthSchemeOptions>
{
    private readonly CookieManager _cookieManager;
    private readonly TokenManager _tokenManager;
    
    public AuthHandler(
        IOptionsMonitor<AuthSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) 
        : base(options, logger, encoder, clock)
    {
        _cookieManager = new CookieManager(options.CurrentValue);
        _tokenManager = new TokenManager(options.CurrentValue);
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
            

        }

        // if (tokenMatch.Success)
        // {
        //     // the token is captured in this group
        //     // as declared in the Regex
        //     var token = tokenMatch.Groups["token"].Value;
        //
        //     try
        //     {
        //         // convert the input token down from Base64 into normal
        //         byte[] fromBase64String = Convert.FromBase64String(token);
        //         var parsedToken = Encoding.UTF8.GetString(fromBase64String);
        //
        //         // deserialize the JSON string obtained from the byte array
        //         model = JsonConvert.DeserializeObject<TokenModel>(parsedToken);
        //     }
        //     catch (System.Exception ex)
        //     {
        //         Console.WriteLine("Exception Occured while Deserializing: " + ex);
        //         return Task.FromResult(AuthenticateResult.Fail("TokenParseException"));
        //     }
        //
        //     // success branch
        //     // generate authTicket
        //     // authenticate the request
        //
        //     /* todo */
        // }
        //
        // // failure branch
        // // return failure
        // // with an optional message
        // return Task.FromResult(AuthenticateResult.Fail("Model is Empty"));
        
        return AuthenticateResult.Fail("Invalid tokens");
    }
}