using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace NotesApi.Auth;

public class NotesAuthHandler : AuthenticationHandler<NotesAuthOptions>
{
    public NotesAuthHandler(IOptionsMonitor<NotesAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder,
        ISystemClock clock) :
        base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        JwtSecurityToken token;

        // validation comes in here
        if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
        {
            return Task.FromResult(AuthenticateResult.Fail("Header Not Found."));
        }

        var header = Request.Headers[HeaderNames.Authorization].ToString();
        // var tokenMatch = Regex.Match(header, AuthSchemeConstants.NToken);


        
        return Task.FromResult(AuthenticateResult.Fail("Model is Empty"));
    }
}