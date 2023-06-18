using System.Net;
using System.Security.Claims;
using Database.User;
using Domain.User;
using DomainBuilder.User;
using NotesApi.RefreshCookieAuthScheme.Cookie;
using NotesApi.RefreshCookieAuthScheme.Token;
using NotesApi.Services.User;
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

    // todo need optimization (used in services)
    private readonly UserManager _userManager;
    
    private readonly IUserRepository _userRepository;

    public AuthManager(IConfiguration configuration)
    {
        _userRepository = new UserRepository(configuration);
        _tokensRepository = new TokensRepository(configuration);
        CookieManager = new CookieManager(configuration);
        TokenManager = new TokenManager(configuration);
        _userManager = new UserManager(configuration);
    }

    public AuthManager(RefreshCookieOptions options)
    {
        _userRepository = new UserRepository(options.ConnectionString);
        _tokensRepository = new TokensRepository(options.ConnectionString);
        CookieManager = new CookieManager(options);
        TokenManager = new TokenManager(options);
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

        var dbTokens = await _tokensRepository.Get(tokens.Token!, tokens.RefreshToken!);

        if (dbTokens == null)
            throw new Exception("Tokens in db not found");

        // check refresh token on alive
        if (dbTokens.CreationDate.AddDays(7) < DateTime.UtcNow)
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
        
        if(tokens == null)
            return;

        var dbTokens = await _tokensRepository.Get(tokens.Token!, tokens.RefreshToken!);

        if(dbTokens == null)
            return;
        
        await _tokensRepository.Delete(dbTokens.Id);
    }

    public async Task SignOutAsync(HttpResponse response)
    {
        var tokens = CookieManager.GetTokens(response.HttpContext);
        
        CookieManager.DeleteTokens(response);
        
        if(tokens == null)
            return;

        var dbTokens = await _tokensRepository.Get(tokens.Token!, tokens.RefreshToken!);

        if(dbTokens == null)
            return;
        
        await _tokensRepository.Delete(dbTokens.Id);
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

    // todo use if used SignOut or refresh tokens
    private async Task DeleteTokens(Tokens tokens)
    {
        var dbTokens = await _tokensRepository.Get(tokens.Token!, tokens.RefreshToken!);

        if (dbTokens == null)
            throw new Exception("Tokens not found");
        
        await _tokensRepository.Delete(dbTokens.Id);
    }
    
    private async Task DeleteTokens(string token, string refreshToken)
    {
        var dbTokens = await _tokensRepository.Get(token, refreshToken);

        if (dbTokens == null)
            throw new Exception("Tokens not found");
        
        await _tokensRepository.Delete(dbTokens.Id);
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