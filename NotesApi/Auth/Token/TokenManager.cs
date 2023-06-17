using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NotesApi.Auth.Token;

public class TokenManager : ITokenManager
{
    public TokenManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TokenManager(AuthSchemeOptions options)
    {
        _options = options;
    }

    private readonly IConfiguration? _configuration;

    private readonly AuthSchemeOptions? _options;

    // jwt constants from appsettings
    private string? Secret =>
        _configuration == null ? _options?.Secret : _configuration["JWT:Secret"];

    private string? ValidAudience =>
        _configuration == null ? _options?.ValidAudience : _configuration["JWT:ValidAudience"];

    private string? ValidIssuer =>
        _configuration == null ? _options?.ValidIssuer : _configuration["JWT:ValidIssuer"];

    private int? TokenValidityInMinutes =>
        _configuration == null
            ? _options?.TokenLifeTimeInMinutes
            : int.Parse(_configuration["JWT:TokenValidityInMinutes"]!);

    private SymmetricSecurityKey IssuerSigningKey =>
        new(Encoding.UTF8.GetBytes(Secret!));

    private SigningCredentials SigningCredentials =>
        new SigningCredentials(IssuerSigningKey, SecurityAlgorithms.HmacSha256);

    #region token parsing

    private TokenValidationParameters GetValidationParameters(bool validateLifeTime = true)
    {
        return new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = IssuerSigningKey,
            ValidateLifetime = validateLifeTime,
            ValidAudience = ValidAudience,
            ValidIssuer = ValidIssuer
        };
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = GetValidationParameters(false);

        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        return claims;
    }

    public ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenValidationParameters = GetValidationParameters();

        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        return claims;
    }

    #endregion

    #region generate token

    public IEnumerable<Claim> CreateIdentityClaims(Guid userId, string email)
    {
        return new List<Claim>()
        {
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Authentication, userId.ToString())
        };
    }

    public JwtSecurityToken GenerateJwtSecurityToken(IEnumerable<Claim> claims)
    {
        if (TokenValidityInMinutes == null)
            throw new Exception("Cannot read token lifetime");
        
        var expires = DateTime.UtcNow.AddMinutes(TokenValidityInMinutes.Value);

        // creating token
        var token = new JwtSecurityToken(
            issuer: ValidIssuer,
            audience: ValidAudience,
            claims: claims,
            expires: expires,
            signingCredentials: SigningCredentials
        );

        return token;
    }

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var token = GenerateJwtSecurityToken(claims);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    #endregion

    #region token lifetime

    public bool TokenActive(string token)
    {
        var tokenTicks = GetTokenExpirationTime(token);

        var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

        var now = DateTime.UtcNow;

        return tokenDate >= now;
    }

    public bool TokenValid(string token)
    {
        var tokenValidationParameters = GetValidationParameters(false);

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }
        catch(Exception e)
        {
            return false;
        }
    }

    private long GetTokenExpirationTime(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
        var ticks = long.Parse(tokenExp);
        return ticks;
    }

    #endregion
}