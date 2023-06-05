using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace NotesApi.Services.Auth;

public class AuthHandler : ISecurityTokenValidator
{
    private readonly JwtSecurityTokenHandler _tokenHandler;
    // private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthHandler()
    {
        // _httpContextAccessor = httpContextAccessor;
        
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public bool CanReadToken(string securityToken)
    {
        return _tokenHandler.CanReadToken(securityToken);
    }

    public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters,
        out SecurityToken validatedToken)
    {
        var principal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);

        return principal;
    }

    public bool CanValidateToken { get; } = true;
    public int MaximumTokenSizeInBytes { get; set; }
}