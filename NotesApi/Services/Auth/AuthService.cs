using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Blank.User;
using Database.User;
using DatabaseBuilder.User;
using Domain.User;
using DomainBuilder.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NotesApi.Services.Interfaces.User;
using Repositories.Repositories.User;
using ViewBuilder.User;
using Views.User;

namespace NotesApi.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    
    private readonly NoteUserRepository _noteUserRepository;
    private readonly TokensRepository _tokensRepository;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
        
        _noteUserRepository = new NoteUserRepository(configuration);
        _tokensRepository = new TokensRepository(configuration);
    }

    public async Task<IActionResult> Login(LoginBlank loginBlank, HttpContext context)
    {
        var user = await GetUser(loginBlank.Email);

        if (user == null)
            return new UnauthorizedResult();
        
        if (HashPassword(loginBlank.Password) != user.PasswordHash)
            return new UnauthorizedResult();

        var tokensView = await CreateTokens(context, user);

        // refactor to cookie manager
        context.Response.Cookies.Append("token", tokensView.Token);
        context.Response.Cookies.Append("refreshToken", tokensView.RefreshToken);
        
        return new OkObjectResult(tokensView);
    }

    public async Task<IActionResult> Registration(UserBlank userBlank, HttpContext context)
    {
        #region check client data

        var user = await GetUser(userBlank.Email);

        if (user != null)
            return new BadRequestObjectResult("Такой email уже зарегистрирован");

        if (!ValidatePassword(userBlank.Password))
            return new BadRequestObjectResult("Неверный формат пароля");
        
        if(!ValidateEmail(userBlank.Email))
            return new BadRequestObjectResult("Неверный формат почты");

        #endregion

        #region generate user and tokens

        var id = await CreateUser(userBlank);

        var tokensView = await CreateTokens(context, id);
        
        #endregion

        // refactor to cookie manager
        context.Response.Cookies.Append("token", tokensView.Token);
        context.Response.Cookies.Append("refreshToken", tokensView.RefreshToken);
        
        return new OkObjectResult(tokensView);
    }

    public async Task<IActionResult> UpdatePassword(ClaimsPrincipal claims, string newPassword, HttpContext context)
    {
        var user = await GetUser(claims);

        if (user == null)
            return new UnauthorizedResult();

        var res = await _noteUserRepository.UpdatePassword(user.Id, HashPassword(newPassword));

        if (res)
            return new BadRequestObjectResult("Не удалось обновить пароль");

        var tokensView = await CreateTokens(context, user);
        
        // refactor to cookie manager
        context.Response.Cookies.Append("token", tokensView.Token);
        context.Response.Cookies.Append("refreshToken", tokensView.RefreshToken);

        return new OkObjectResult(tokensView);
    }
    
    // need refactor
    public async Task<IActionResult> RefreshTokens(TokensBlank tokens, HttpContext context)
    {
        var token = tokens.Token;
        var refreshToken = tokens.RefreshToken;

        #region tokens db and their validation

        var tokensDb = await _tokensRepository.Get(token, refreshToken);

        if (tokensDb == null)
            return new BadRequestObjectResult("Token not valid");

        // check expire date
        if (tokensDb.CreationDate.AddDays(7) < DateTime.Now)
            return new BadRequestObjectResult("Refresh token expired");

        if (!tokensDb.Active)
            return new BadRequestObjectResult("Token not active");

        #endregion

        var user = await _noteUserRepository.Get(GetUsername(token)!);

        if (user == null)
            return new BadRequestObjectResult("User not found");
        
        #region generate and save tokens

        var clientIp = GetIpAddress(context.Request.Host.Host);

        var newTokens = GenerateTokens(user.Id, user.Email, clientIp);

        await SaveTokens(newTokens);

        await _tokensRepository.SetNotActive(tokensDb.Id);

        var tokensDomain = TokensDomainBuilder.Create(newTokens);

        var tokensView = TokensViewBuilder.Create(tokensDomain);

        #endregion
        
        // refactor to cookie manager
        context.Response.Cookies.Append("token", tokensView.Token);
        context.Response.Cookies.Append("refreshToken", tokensView.RefreshToken);

        return new OkObjectResult(tokensView);
    }

    #region save in db
    
    private async Task<bool> SaveTokens(TokensDatabase tokensDatabase)
    {
        return await _tokensRepository.Create(tokensDatabase) > 0;
    }
    
    private async Task<int> CreateUser(UserBlank userBlank)
    {
        string hashedPassword = HashPassword(userBlank.Password);

        var newUser = UserDatabaseBuilder.Create(userBlank, hashedPassword);

        return await _noteUserRepository.Create(newUser);
    }
    
    #endregion

    #region generating data

    // generate pair tokens
    private TokensDatabase GenerateTokens(int userId, string email, IPAddress? ipAddress)
    {
        var claims = CreateClaims(email);

        string token = new JwtSecurityTokenHandler()
            .WriteToken(GenerateToken(claims));

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
    
    // returns claims with user email
    private IEnumerable<Claim> CreateClaims(string email)
    {
        return new List<Claim>()
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, email)
        };
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
            expires: DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
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

    #endregion

    #region parsing

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

    #endregion
    
    #region validation

    // validate password 
    private bool ValidatePassword(string password) =>
        password.Any(char.IsLetter) &&
        password.Any(char.IsDigit) &&
        password.Any(char.IsUpper) &&
        password.Any(char.IsLower) &&
        password.Length >= 8;

    // validate email
    private bool ValidateEmail(string email) =>
        new EmailAddressAttribute().IsValid(email);

    #endregion
    
    private async Task<UserDomain?> GetUser(ClaimsPrincipal claims)
    {
        var email = claims.Identity?.Name;

        if (string.IsNullOrEmpty(email))
            return null;

        var user = await _noteUserRepository.Get(email);

        return UserDomainBuilder.Create(user);
    }
    
    private async Task<UserDomain?> GetUser(string email)
    {
        var user = await _noteUserRepository.Get(email);

        return UserDomainBuilder.Create(user);
    }
    
    private async Task<UserDomain?> GetUser(int id)
    {
        var user = await _noteUserRepository.Get(id);

        return UserDomainBuilder.Create(user);
    }

    private async Task<TokensView> CreateTokens(HttpContext context, UserDomain user)
    {
        var clientIp = GetIpAddress(context.Request.Host.Host);

        var tokens = GenerateTokens(user.Id, user.Email, clientIp);

        await SaveTokens(tokens);

        var tokensDomain = TokensDomainBuilder.Create(tokens);

        var tokensView = TokensViewBuilder.Create(tokensDomain);

        return tokensView;
    }
    
    private async Task<TokensView> CreateTokens(HttpContext context, int id)
    {
        var user = await GetUser(id);

        if (user == null)
            throw new Exception("Undefined user");
        
        var clientIp = GetIpAddress(context.Request.Host.Host);

        var tokens = GenerateTokens(user.Id, user.Email, clientIp);

        await SaveTokens(tokens);

        var tokensDomain = TokensDomainBuilder.Create(tokens);

        var tokensView = TokensViewBuilder.Create(tokensDomain);

        return tokensView;
    }
}