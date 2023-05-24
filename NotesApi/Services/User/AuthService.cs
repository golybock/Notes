using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using Blank.User;
using Database.User;
using DatabaseBuilder.User;
using DomainBuilder.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NotesApi.Repositories.User;
using NotesApi.Services.Interfaces.User;
using ViewBuilder.User;

namespace NotesApi.Services.User;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly UserRepository _userRepository;
    private readonly TokensRepository _tokensRepository;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
        _userRepository = new UserRepository(configuration);
        _tokensRepository = new TokensRepository(configuration);
    }

    public string? GetEmail(string token)
    {
        try
        {
            return GetEmailFromClaims(GetPrincipalFromExpiredToken(token)?.Claims);
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<IActionResult> Login(string email, string password, HttpContext context)
    {
        #region check client

        var client = await _userRepository.Get(email);

        if (client == null)
            return new UnauthorizedObjectResult("Неверный логин или пароль");

        if (HashPassword(password) != client.PasswordHash)
            return new UnauthorizedObjectResult("Неверный пароль");

        #endregion

        #region generate and save tokens

        IPAddress clientIp = IPAddress.Parse(context.Request.Host.Host);

        var tokens = GenerateTokens(client.Id, client.Email, clientIp);

        var save = await SaveTokens(tokens);

        if (save == null)
            return new BadRequestResult();

        var tokensDomain = TokensDomainBuilder.Create(tokens);

        var tokensView = TokensViewBuilder.Create(tokensDomain);

        #endregion

        return new OkObjectResult(tokensView);
    }

    public async Task<IActionResult> Registration(UserBlank userBlank, HttpContext context)
    {
        #region check client data

        var client = await _userRepository.Get(userBlank.Email);

        if (client != null)
            return new BadRequestObjectResult("Такой email уже зарегистрирован");

        if (!ValidatePassword(userBlank.Password))
            return new BadRequestObjectResult("Неверный формат пароля");

        #endregion

        #region generate user and tokens

        var id = await CreateUser(userBlank);

        var clientIp = GetIpAddress(context.Request.Host.Host);

        var tokens = GenerateTokens(id, userBlank.Email, clientIp);

        var save = await SaveTokens(tokens);

        if (save == null)
            return new BadRequestResult();

        var tokensDomain = TokensDomainBuilder.Create(tokens);

        var tokensView = TokensViewBuilder.Create(tokensDomain);

        #endregion

        return new OkObjectResult(tokensView);
    }

    public async Task<IActionResult> UpdatePassword(IEnumerable<Claim> claims, string newPassword)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> RefreshTokens(TokensBlank tokens)
    {
        throw new NotImplementedException();
    }

    private TokensDatabase GenerateTokens(int userId, string email, IPAddress ipAddress)
    {
        var claims = CreateClaims(email);

        string token = new JwtSecurityTokenHandler()
            .WriteToken(
                GenerateToken(claims)
            );

        string refreshToken = GetRefreshTokens();

        var tokenDatabase = new TokensDatabase()
        {
            UserId = userId,
            Active = true,
            Ip = ipAddress,
            Token = token,
            RefreshToken = refreshToken
        };

        return tokenDatabase;
    }

    private async Task<int?> SaveTokens(TokensDatabase tokensDatabase)
    {
        try
        {
            return await _tokensRepository.Create(tokensDatabase);
        }
        catch (Exception e)
        {
            return null;
        }
    }

    private IEnumerable<Claim> CreateClaims(string email)
    {
        return new List<Claim>() {new Claim(ClaimTypes.Email, email)};
    }

    private string? GetEmailFromClaims(IEnumerable<Claim> claims)
    {
        return claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
    }

    private async Task<int> CreateUser(UserBlank userBlank)
    {
        string hashedPassword = HashPassword(userBlank.Password);

        var newUser = UserDatabaseBuilder.Create(userBlank, hashedPassword);

        return await _userRepository.Create(newUser);
    }

    private string GetRefreshTokens()
    {
        return Guid.NewGuid().ToString();
    }

    private JwtSecurityToken GenerateToken(IEnumerable<Claim> claims)
    {
        string secret = _configuration["JWT:Secret"]!;
        string validIssuer = _configuration["JWT:ValidIssuer"]!;
        string validAudience = _configuration["JWT:ValidAudience"]!;
        int tokenValidityInMinutes = int.Parse(_configuration["JWT:TokenValidityInMinutes"]!);

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var token = new JwtSecurityToken(
            issuer: validIssuer,
            audience: validAudience,
            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    private string HashPassword(string password)
    {
        byte[] input = Encoding.UTF8.GetBytes(password);

        using var md5 = MD5.Create();
        var output = md5.ComputeHash(input);

        return Convert.ToBase64String(output);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

    private IPAddress? GetIpAddress(string ip)
    {
        try
        {
            return IPAddress.Parse(ip);
        }
        catch (Exception e)
        {
            return null;
        }
    }

    private bool ValidatePassword(string password)
    {
        return password.Any(char.IsLetter) &&
               password.Any(char.IsDigit) &&
               password.Any(char.IsUpper) &&
               password.Any(char.IsLower) &&
               password.Length >= 8;
    }
}