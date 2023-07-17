using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NotesApi.RefreshCookieAuthScheme.Token;

public class TokenManager : ITokenManager
{
    private readonly RefreshCookieOptions _options;
    
    public TokenManager(RefreshCookieOptions options)
    {
        _options = options;
    }

    #region validate params from appsettings or options

    private string? Secret => _options.Secret;

    private string? ValidAudience => _options.ValidAudience;

    private string? ValidIssuer => _options.ValidIssuer;

    private int? TokenValidityInMinutes => _options.TokenLifeTimeInMinutes;

    private SymmetricSecurityKey IssuerSigningKey =>
        new(Encoding.UTF8.GetBytes(Secret!));

    private SigningCredentials SigningCredentials =>
        new(IssuerSigningKey, SecurityAlgorithms.HmacSha256);

    #endregion

    #region validate

    private bool ValidateIssuerSigningKey =>
        _options.ValidateIssuerSigningKey;

    private bool ValidateLifetime =>
        _options.ValidateLifetime;

    private bool ValidateIssuer =>
        _options.ValidateIssuer;

    private bool ValidateAudience =>
        _options.ValidateAudience;

    #endregion

    #region token parsing

    private TokenValidationParameters GetValidationParameters(bool validateLifeTime = true)
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = ValidateIssuerSigningKey,
            IssuerSigningKey = IssuerSigningKey,
            ValidateLifetime = validateLifeTime,
            ValidateAudience = ValidateAudience,
            ValidateIssuer = ValidateIssuer,
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
        var tokenValidationParameters = GetValidationParameters(ValidateLifetime);

        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        return claims;
    }

    public Guid GetUserIdFromToken(string token)
    {
        var claims = GetPrincipalFromToken(token);
        
        var id = claims.FindFirst(ClaimTypes.Authentication)?.Value;

        return Guid.Parse(id!);
    }

    public string? GetEmailFromToken(string token)
    {
        var claims = GetPrincipalFromToken(token);
        
        var email = claims.FindFirst(ClaimTypes.Name)?.Value;

        return email;
    }

    public Guid GetUserIdFromClaims(ClaimsPrincipal claims)
    {
        var id = claims.FindFirst(ClaimTypes.Authentication)?.Value;

        return Guid.Parse(id!);
    }

    public string? GetEmailFromClaims(ClaimsPrincipal claims)
    {
        var email = claims.FindFirst(ClaimTypes.Name)?.Value;

        return email;
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
        DateTime expires = new DateTime();

        // если включена валидация по времени
        if (ValidateLifetime)
        {
            // время не указано
            if (TokenValidityInMinutes == null)
                throw new Exception("Cannot read TokenValidityInMinutes, but validatingLifeTime = true");

            expires = DateTime.UtcNow.AddMinutes(TokenValidityInMinutes.Value);
        }

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

    public string GenerateRefreshToken() =>
        Guid.NewGuid().ToString();

    #endregion

    #region token lifetime

    public bool TokenActive(string token)
    {
        var validTo = GetTokenExpirationTime(token);

        var now = DateTime.UtcNow;

        return validTo >= now;
    }

    // todo refactor
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

    private DateTime GetTokenExpirationTime(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = handler.ReadJwtToken(token);

        return jwtSecurityToken.ValidTo;
    }

    #endregion
}