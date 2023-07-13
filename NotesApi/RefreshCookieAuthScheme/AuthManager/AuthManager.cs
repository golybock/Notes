using System.Net;
using System.Security.Claims;
using Database.User;
using Domain.User;
using NotesApi.RefreshCookieAuthScheme.CacheService;
using NotesApi.RefreshCookieAuthScheme.Token;
using NotesApi.Services.User;
using Repositories.Interfaces.User;
using ICookieManager = NotesApi.RefreshCookieAuthScheme.Cookie.ICookieManager;

namespace NotesApi.RefreshCookieAuthScheme.AuthManager;

public class AuthManager : IAuthManager
{
    public ICookieManager CookieManager { get; set; }
    public ITokenManager TokenManager { get; set; }
    public ITokenCacheService TokenCacheService { get; set; }

    private readonly UserManager _userManager;
    
    public RefreshCookieOptions? Options { get; set; }

    public AuthManager(IConfiguration configuration,
        ICookieManager cookieManager,
        ITokenManager tokenManager,
        ITokenCacheService tokenCacheService)
    {
        CookieManager = cookieManager;
        TokenManager = tokenManager;
        TokenCacheService = tokenCacheService;
        _userManager = new UserManager(configuration);
    }

    // override IpAddress.Parse
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

    // signIn
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
        var user = await _userManager.GetUser(principal);

        var tokens = CreateTokens(user);

        await SaveTokensAsync(response.HttpContext, tokens, user.Id);

        CookieManager.SetTokens(response, tokens);
    }

    public async Task SignInAsync(HttpContext context, ClaimsPrincipal principal)
    {
        var user = await _userManager.GetUser(principal);

        var tokens = CreateTokens(user);

        await SaveTokensAsync(context, tokens, user.Id);

        CookieManager.SetTokens(context, tokens);
    }

    public async Task RefreshTokensAsync(HttpResponse response, ClaimsPrincipal claimsPrincipal, Tokens tokens)
    {
        var user = await _userManager.GetUser(claimsPrincipal);

        var cachedTokens = await TokenCacheService.GetTokens(user.Id, tokens.RefreshToken);
        
        if (cachedTokens == null)
            throw new Exception("Tokens in cache not found");

        // todo refactor to get time from settings
        // check refresh token on alive
        if (cachedTokens.CreationDate.AddDays(7) < DateTime.UtcNow)
        {
            await SignOutAsync(response);

            throw new Exception("Refresh token died");
        }

        await SignInAsync(response, user);
    }

    public async Task SignOutAsync(HttpContext context)
    {
        var tokens = CookieManager.GetTokens(context);

        CookieManager.DeleteTokens(context);

        if (tokens == null)
            return;

        await DeleteTokensCache(tokens);
    }

    public async Task SignOutAsync(HttpResponse response)
    {
        var tokens = CookieManager.GetTokens(response.HttpContext);

        CookieManager.DeleteTokens(response);

        if (tokens == null)
            return;

        await DeleteTokensCache(tokens);
    }

    private async Task SaveTokensAsync(HttpContext context, Tokens tokens, Guid userId)
    {
        var tokensDatabase = new TokensDatabase()
        {
            Token = tokens.Token,
            RefreshToken = tokens.RefreshToken,
            Ip = GetIpAddress(context.Request.Host.Host),
            UserId = userId,
            CreationDate = DateTime.UtcNow
        };

        if (Options == null)
            throw new Exception("Options is null");
        
        await TokenCacheService.SetTokens(userId, tokensDatabase, Options.RefreshTokenLifeTime);
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

    // get user from claims and delete pair user:refreshToken from cache
    private async Task DeleteTokensCache(Tokens tokens)
    {
        var claims = TokenManager.GetPrincipalFromToken(tokens.Token);
        
        var user = await _userManager.GetUser(claims);

        await TokenCacheService.DeleteTokens(user.Id ,tokens.RefreshToken);
    }
}