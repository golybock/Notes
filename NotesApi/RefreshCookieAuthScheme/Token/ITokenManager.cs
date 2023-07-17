using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NotesApi.RefreshCookieAuthScheme.Token;

public interface ITokenManager
{
    public bool TokenActive(string token);

    public bool TokenValid(string token);
    
    public string GenerateRefreshToken();
    
    public string GenerateToken(IEnumerable<Claim> claims);

    public ClaimsPrincipal GetPrincipalFromToken(string token);
    
    public Guid GetUserIdFromToken(string token);
    
    public string? GetEmailFromToken(string token);
    
    public Guid GetUserIdFromClaims(ClaimsPrincipal claims);
    
    public string? GetEmailFromClaims(ClaimsPrincipal claims);

    public IEnumerable<Claim> CreateIdentityClaims(Guid userId, string email);

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    
    protected JwtSecurityToken GenerateJwtSecurityToken(IEnumerable<Claim> claims);
}