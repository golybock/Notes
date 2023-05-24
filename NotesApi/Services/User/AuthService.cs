using System.ComponentModel.DataAnnotations;
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

        var clientIp = GetIpAddress(context.Request.Host.Host);

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

    public async Task<IActionResult> UpdatePassword(ClaimsPrincipal claimsPrincipal, string newPassword,
        HttpContext context)
    {
        var user = await _userRepository.Get(claimsPrincipal?.Identity?.Name);

        if (user == null)
            return new UnauthorizedResult();

        await _userRepository.UpdatePassword(user.Id, HashPassword(newPassword));

        #region generate and save tokens

        var clientIp = GetIpAddress(context.Request.Host.Host);

        var newTokens = GenerateTokens(user.Id, user.Email, clientIp);

        var save = await SaveTokens(newTokens);

        if (save == null)
            return new BadRequestResult();

        var tokensDomain = TokensDomainBuilder.Create(newTokens);

        var tokensView = TokensViewBuilder.Create(tokensDomain);

        #endregion

        return new OkObjectResult(tokensView);
    }

    // refresh tokens
    public async Task<IActionResult> RefreshTokens(TokensBlank tokens, HttpContext context)
    {
        var username = GetUsername(tokens.Token);

        if (username == null)
            return new BadRequestObjectResult("User not found");

        var refreshToken = tokens.RefreshToken;

        #region tokens db and their validation

        var tokensDb = await _tokensRepository.Get(refreshToken);

        if (tokensDb == null)
            return new BadRequestObjectResult("Token not valid");

        // check expire date
        if (tokensDb.CreationDate.AddDays(7) < DateTime.Now)
            return new BadRequestObjectResult("Refresh token expired");

        if (!tokensDb.Active)
            return new BadRequestObjectResult("Token not active");

        #endregion

        #region user db and their validation

        var userDb = await _userRepository.Get(username);

        if (userDb == null)
            return new UnauthorizedObjectResult("User not found");

        if (tokensDb.UserId != userDb.Id)
            return new BadRequestObjectResult("Refresh token invalid");

        #endregion

        #region generate and save tokens

        var clientIp = GetIpAddress(context.Request.Host.Host);

        var newTokens = GenerateTokens(userDb.Id, userDb.Email, clientIp);

        var save = await SaveTokens(newTokens);

        await _tokensRepository.SetNotActive(refreshToken);

        if (save == null)
            return new BadRequestResult();

        var tokensDomain = TokensDomainBuilder.Create(newTokens);

        var tokensView = TokensViewBuilder.Create(tokensDomain);

        #endregion

        return new OkObjectResult(tokensView);
    }

    // generate pair tokens
    private TokensDatabase GenerateTokens(int userId, string email, IPAddress? ipAddress)
    {
        var claims = CreateClaims(email);

        string token = new JwtSecurityTokenHandler()
            .WriteToken(
                GenerateToken(claims)
            );

        string refreshToken = GetRefreshToken();

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

    // save tokens in db and returns id
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

    // returns claims with user email
    private IEnumerable<Claim> CreateClaims(string email)
    {
        return new List<Claim>()
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, email)
        };
    }

    // creating user in db
    private async Task<int> CreateUser(UserBlank userBlank)
    {
        string hashedPassword = HashPassword(userBlank.Password);

        var newUser = UserDatabaseBuilder.Create(userBlank, hashedPassword);

        return await _userRepository.Create(newUser);
    }

    // generate guid in string format
    private string GetRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    // generate jwt-token
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

    // md5 hash password
    private string HashPassword(string password)
    {
        byte[] input = Encoding.UTF8.GetBytes(password);

        using var md5 = MD5.Create();
        var output = md5.ComputeHash(input);

        return Convert.ToBase64String(output);
    }

    // returns user email from expired token
    private string? GetUsername(string token)
    {
        try
        {
            var principal = GetPrincipalFromExpiredToken(token);

            return principal?.Identity?.Name;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    // returns principal from expired token
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        string secret = _configuration["JWT:Secret"]!;

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
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

    // try parse ipAddress
    private IPAddress? GetIpAddress(string ip)
    {
        try
        {
            if (ip == "localhost")
                return IPAddress.Parse("127.0.0.1");

            return IPAddress.Parse(ip);
        }
        catch (Exception e)
        {
            return null;
        }
    }

    // validate password 
    private bool ValidatePassword(string password)
    {
        return password.Any(char.IsLetter) &&
               password.Any(char.IsDigit) &&
               password.Any(char.IsUpper) &&
               password.Any(char.IsLower) &&
               password.Length >= 8;
    }

    // validate email
    private bool ValidateEmail(string email)
    {
        return new EmailAddressAttribute().IsValid(email);
    }
}