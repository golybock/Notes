using System.Security.Claims;
using Domain.User;
using NotesApi.RefreshCookieAuthScheme.CacheService;
using NotesApi.RefreshCookieAuthScheme.Cookie;
using NotesApi.RefreshCookieAuthScheme.Token;
using ICookieManager = NotesApi.RefreshCookieAuthScheme.Cookie.ICookieManager;

namespace NotesApi.RefreshCookieAuthScheme.AuthManager;

public class AuthManager : IAuthManager
{
    public ICookieManager CookieManager { get; set; }
    public ITokenManager TokenManager { get; set; }
    public ITokenCacheService TokenCacheService { get; set; }

    // auth scheme options
    private RefreshCookieOptions Options { get; set; }

    public AuthManager(RefreshCookieOptions options, ITokenCacheService tokenCacheService, ITokenManager tokenManager)
    {
        Options = options;
        TokenCacheService = tokenCacheService;
        TokenManager = tokenManager;
        CookieManager = new CookieManager();
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
        var tokens = CreateTokens(user.Id, user.Email);

        await SaveTokensAsync(context, tokens, user.Id);

        CookieManager.SetTokens(context, tokens, Options.RefreshTokenLifeTimeInDays);
    }

    public async Task SignInAsync(HttpResponse response, UserDomain user)
    {
        var tokens = CreateTokens(user.Id, user.Email);

        await SaveTokensAsync(response.HttpContext, tokens, user.Id);

        CookieManager.SetTokens(response, tokens, Options.RefreshTokenLifeTimeInDays);
    }

    public async Task SignInAsync(HttpResponse response, ClaimsPrincipal claims)
    {
        var userId = TokenManager.GetUserIdFromClaims(claims);

        var userEmail = TokenManager.GetEmailFromClaims(claims);
        
        var tokens = CreateTokens(userId, userEmail!);

        await SaveTokensAsync(response.HttpContext, tokens, userId);

        CookieManager.SetTokens(response, tokens, Options.RefreshTokenLifeTimeInDays);
    }

    public async Task SignInAsync(HttpContext context, ClaimsPrincipal claims)
    {
        var userId = TokenManager.GetUserIdFromClaims(claims);

        var userEmail = TokenManager.GetEmailFromClaims(claims);

        var tokens = CreateTokens(userId, userEmail!);

        await SaveTokensAsync(context, tokens, userId);

        CookieManager.SetTokens(context, tokens, Options.RefreshTokenLifeTimeInDays);
    }

    public async Task RefreshTokensAsync(HttpResponse response, Tokens tokens)
    {
        var userId = TokenManager.GetUserIdFromToken(tokens.Token);

        var claims = TokenManager.GetPrincipalFromExpiredToken(tokens.Token);

        var cachedTokens = await TokenCacheService.GetTokens(userId, tokens.RefreshToken);

        if (cachedTokens == null)
            throw new Exception("Tokens in cache not found");

        await DeleteTokensCache(cachedTokens, userId);

        await SignInAsync(response, claims);
    }

    public async Task SignOutAsync(HttpContext context)
    {
        var tokens = CookieManager.GetTokens(context);

        CookieManager.DeleteTokens(context);

        if (tokens == null)
            return;

        var userId = TokenManager.GetUserIdFromToken(tokens.Token);

        await DeleteTokensCache(tokens, userId);
    }

    public async Task SignOutAsync(HttpResponse response)
    {
        var tokens = CookieManager.GetTokens(response.HttpContext);

        CookieManager.DeleteTokens(response);

        if (tokens == null)
            return;

        var userId = TokenManager.GetUserIdFromToken(tokens.Token);

        await DeleteTokensCache(tokens, userId);
    }

    private async Task SaveTokensAsync(HttpContext context, Tokens tokens, Guid userId)
    {
        var tokensDatabase = new TokensModel()
        {
            Token = tokens.Token,
            RefreshToken = tokens.RefreshToken,
            Ip = GetIpAddress(context.Request.Host.Host),
            UserId = userId,
        };

        await TokenCacheService.SetTokens(userId, tokensDatabase, Options.RefreshTokenLifeTime);
    }

    private Tokens CreateTokens(Guid userId, string userEmail)
    {
        var claims = TokenManager.CreateIdentityClaims(userId, userEmail);

        var token = TokenManager.GenerateToken(claims);
        var refreshToken = TokenManager.GenerateRefreshToken();

        var tokens = new Tokens()
        {
            Token = token,
            RefreshToken = refreshToken
        };

        return tokens;
    }

    // delete pair with key 'user:refreshToken' from cache
    private Task DeleteTokensCache(Tokens tokens, Guid userId) => 
        TokenCacheService.DeleteTokens(userId, tokens.RefreshToken);

    private Task DeleteTokensCache(TokensModel tokensModel, Guid userId) => 
        TokenCacheService.DeleteTokens(userId, tokensModel.RefreshToken);
}