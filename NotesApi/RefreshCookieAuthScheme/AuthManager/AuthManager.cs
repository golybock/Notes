using System.Security.Claims;
using Database.User;
using Domain.User;
using Microsoft.Extensions.Options;
using NotesApi.RefreshCookieAuthScheme.CacheService;
using NotesApi.RefreshCookieAuthScheme.Token;
using NotesApi.Services.User.UserManager;
using ICookieManager = NotesApi.RefreshCookieAuthScheme.Cookie.ICookieManager;

namespace NotesApi.RefreshCookieAuthScheme.AuthManager;

public class AuthManager : IAuthManager
{
    public ICookieManager CookieManager { get; set; }
    public ITokenManager TokenManager { get; set; }
    public ITokenCacheService TokenCacheService { get; set; }

    // todo refactor to di
    private readonly UserManager _userManager;

    private RefreshCookieOptions Options { get; set; }

    public AuthManager(IConfiguration configuration,
        ICookieManager cookieManager,
        ITokenCacheService tokenCacheService,
        IOptions<RefreshCookieOptions> options,
        ITokenManager tokenManager)
    {
        CookieManager = cookieManager;
        Options = options.Value;
        TokenCacheService = tokenCacheService;
        TokenManager = tokenManager;
        _userManager = new UserManager(configuration);
    }

    // override IpAddress.Parse
    private string? GetIpAddress(string ip)
    {
        try
        {
            if (ip == "localhost")
                return "127.0.0.1";

            return ip;
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

        await DeleteTokensCache(cachedTokens);

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
        };

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

        await TokenCacheService.DeleteTokens(user.Id, tokens.RefreshToken);
    }

    private async Task DeleteTokensCache(TokensDatabase tokensDatabase)
    {
        var claims = TokenManager.GetPrincipalFromToken(tokensDatabase.Token);

        var user = await _userManager.GetUser(claims);

        await TokenCacheService.DeleteTokens(user.Id, tokensDatabase.RefreshToken);
    }
}