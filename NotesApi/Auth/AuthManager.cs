using System.Net;
using System.Security.Claims;
using Database.User;
using Domain.User;
using DomainBuilder.User;
using NotesApi.Auth.Cookie;
using NotesApi.Auth.Token;
using Repositories.Repositories.User;

namespace NotesApi.Auth;

public class AuthManager : IAuthManager
{
    public CookieManager CookieManager { get; set; }
    public TokenManager TokenManager { get; set; }

    // database
    private readonly TokensRepository _tokensRepository;
    
    private readonly UserRepository _userRepository;

    public AuthManager(IConfiguration configuration)
    {
        _userRepository = new UserRepository(configuration);
        _tokensRepository = new TokensRepository(configuration);
        CookieManager = new CookieManager(configuration);
        TokenManager = new TokenManager(configuration);
    }

    public AuthManager(AuthSchemeOptions options)
    {
        _userRepository = new UserRepository(options.ConnectionString);
        _tokensRepository = new TokensRepository(options.ConnectionString);
        CookieManager = new CookieManager(options);
        TokenManager = new TokenManager(options);
    }

    private async Task<UserDomain?> GetUser(ClaimsPrincipal claims)
    {
        var id = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (id == null)
            return null;

        var user = await _userRepository.Get(id);

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