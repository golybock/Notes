using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NotesApi.Auth.Token;

public interface ITokenManager
{
    public bool TokenActive(string token);

    public bool TokenValid(string token);
    
    public string GenerateRefreshToken();
    
    public string GenerateToken(IEnumerable<Claim> claims);

    public ClaimsPrincipal GetPrincipalFromToken(string token);

    public IEnumerable<Claim> CreateIdentityClaims(Guid userId, string email);

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    
    protected JwtSecurityToken GenerateJwtSecurityToken(IEnumerable<Claim> claims);
}