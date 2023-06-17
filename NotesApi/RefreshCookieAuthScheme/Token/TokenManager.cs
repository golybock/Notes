using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NotesApi.RefreshCookieAuthScheme.Token;

public class TokenManager : ITokenManager
{
    public TokenManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TokenManager(RefreshCookieOptions options)
    {
        _options = options;
    }

    private readonly IConfiguration? _configuration;

    private readonly RefreshCookieOptions? _options;

    private string ErrorNotFound =>
        "Token manager: options and configuration not found";

    // jwt const from appsettings or options
    private string? Secret
    {
        get
        {
            if (_configuration != null)
                return _configuration["JWT:Secret"];

            if (_options == null)
                throw new Exception(ErrorNotFound);

            return _options.Secret;
        }
    }

    private string? ValidAudience
    {
        get
        {
            if (_configuration != null)
                return _configuration["JWT:ValidAudience"];

            if (_options == null)
                throw new Exception(ErrorNotFound);

            return _options.ValidAudience;
        }
    }

    private string? ValidIssuer
    {
        get
        {
            if (_configuration != null)
                return _configuration["JWT:ValidIssuer"];

            if (_options == null)
                throw new Exception(ErrorNotFound);

            return _options.ValidIssuer;
        }
    }

    private int? TokenValidityInMinutes
    {
        get
        {
            if (_configuration != null)
                return int.Parse(_configuration["JWT:TokenValidityInMinutes"] ??
                                 throw new Exception("TokenValidityInMinutes Not found"));

            if (_options == null)
                throw new Exception(ErrorNotFound);

            return _options.TokenLifeTimeInMinutes;
        }
    }

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
        catch (Exception e)
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