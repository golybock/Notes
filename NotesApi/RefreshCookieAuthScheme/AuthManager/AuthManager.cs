using System.Net;
using System.Security.Claims;
using Database.User;
using Domain.User;
using DomainBuilder.User;
using NotesApi.RefreshCookieAuthScheme.Cookie;
using NotesApi.RefreshCookieAuthScheme.Token;
using Repositories.Repositories.Interfaces.User;
using Repositories.Repositories.User;
using ICookieManager = NotesApi.RefreshCookieAuthScheme.Cookie.ICookieManager;

namespace NotesApi.RefreshCookieAuthScheme.AuthManager;

public class AuthManager : IAuthManager
{
    public ICookieManager CookieManager { get; set; }
    public ITokenManager TokenManager { get; set; }

    // database
    private readonly ITokenRepository _tokensRepository;
    
    private readonly IUserRepository _userRepository;

    public AuthManager(IConfiguration configuration)
    {
        _userRepository = new UserRepository(configuration);
        _tokensRepository = new TokensRepository(configuration);
        CookieManager = new CookieManager(configuration);
        TokenManager = new TokenManager(configuration);
    }

    public AuthManager(RefreshCookieOptions options)
    {
        _userRepository = new UserRepository(options.ConnectionString);
        _tokensRepository = new TokensRepository(options.ConnectionString);
        CookieManager = new CookieManager(options);
        TokenManager = new TokenManager(options);
    }

    public async Task<UserDomain?> GetUser(ClaimsPrincipal claims)
    {
        var id = claims.FindFirst(ClaimTypes.Authentication)?.Value;

        if (id == null)
            return null;

        Guid guid = Guid.Parse(id);
        
        var user = await _userRepository.Get(guid);

        if (user == null)
            return null;

        return UserDomainBuilder.Create(user);
    }

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

    public async Task SignInAsync(HttpContext context, UserDomain user)
    {
        var tokens = CreateTokens(user);

        await SaveTokensAsync(context, tokens, user.Id);

        CookieManager.SetTokens(context, tokens);
    }

    public async Task SignInAsync(HttpResponse response, UserDomain user)
    {
        var tokens = CreateTokens(user);

        await SaveTokensAsync(response.HttpContext, tokens, user.Id);

        CookieManager.SetTokens(response, tokens);
    }

    public async Task SignInAsync(HttpResponse response, ClaimsPrincipal principal)
    {
        var user = await GetUser(principal);

        if (user == null)
            throw new Exception("user not found in principal");

        var tokens = CreateTokens(user);

        await SaveTokensAsync(response.HttpContext, tokens, user.Id);

        CookieManager.SetTokens(response, tokens);
    }

    public async Task SignInAsync(HttpContext context, ClaimsPrincipal principal)
    {
        throw new NotImplementedException();
    }

    public void SignOut(HttpContext context)
    {
        CookieManager.DeleteTokens(context);
    }

    public void SignOut(HttpResponse response)
    {
        CookieManager.DeleteTokens(response);
    }

    private async Task SaveTokensAsync(HttpContext context, Tokens tokens, Guid userId)
    {
        var tokensDatabase = new TokensDatabase()
        {
            Token = tokens.Token!,
            RefreshToken = tokens.RefreshToken!,
            Ip = GetIpAddress(context.Request.Host.Host),
            Active = true,
            UserId = userId,
            CreationDate = DateTime.UtcNow
        };

        await _tokensRepository.Create(tokensDatabase);
    }

    private Tokens CreateTokens(UserDomain user)
    {
        var claims = TokenManager.CreateIdentityClaims(user.Id, user.Email);

        var token = TokenManager.GenerateToken(claims);
        var refreshToken = TokenManager.GenerateRefreshToken();

        var tokens = new Tokens()
        {
            Token = token,
            RefreshToken = refreshToken
        };

        return tokens;
    }
}