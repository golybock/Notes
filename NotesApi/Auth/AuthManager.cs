using System.Net;
using System.Security.Claims;
using Blank.User;
using Database.User;
using Domain.User;
using DomainBuilder.User;
using NotesApi.Auth.Cookie;
using NotesApi.Auth.Tokens;
using Repositories.Repositories.User;

namespace NotesApi.Auth;

public class AuthManager
{
    private readonly CookieManager _cookieManager;
    private readonly TokenManager _tokenManager;

    private readonly TokensRepository _tokensRepository;
    private readonly UserRepository _userRepository;

    // private HttpContext _context;

    public AuthManager(IConfiguration configuration)
    {
        // _context = context;

        _userRepository = new UserRepository(configuration);
        _tokenManager = new TokenManager(configuration);
        _tokensRepository = new TokensRepository(configuration);
        _cookieManager = new CookieManager(configuration);
    }

    // set new tokens in cookie and save it in db
    public async Task SignInAsync(HttpContext context, UserDomain userDomain)
    {
        var email = userDomain.Email;

        var claims = _tokenManager.CreateIdentityClaims(email);

        var token = _tokenManager.GenerateToken(claims);
        var refreshToken = _tokenManager.GenerateRefreshToken();

        var tokens = new Blank.User.Tokens()
        {
            Token = token,
            RefreshToken = refreshToken
        };

        await SaveTokensAsync(context, token, refreshToken, userDomain.Id);

        _cookieManager.SetTokens(context, tokens);
    }
    
    public void SignOut(HttpContext context) => _cookieManager.DeleteTokens(context);

    private async Task<UserDomain?> GetUser(string email)
    {
        var user = await _userRepository.Get(email);

        return UserDomainBuilder.Create(user);
    }

    public async Task<UserDomain?> IsSigned(HttpContext context)
    {
        var tokens = GetTokens(context);

        if (tokens == null)
            return null;

        var user = await GetCurrentUser(context);

        if (user == null)
            return null;

        // token died
        if (!_tokenManager.TokenActive(tokens.Token))
        {
            await SignInAsync(context, user);
            await SetTokensNotActive(tokens);

            return user;
        }

        return user;
    }

    // override for simply
    private Blank.User.Tokens? GetTokens(HttpContext context)
    {
        try
        {
            var tokens = _cookieManager.GetTokens(context);

            return tokens;
        }
        catch
        {
            return null;
        }
    }

    private async Task<bool> SetTokensNotActive(Blank.User.Tokens tokensDomain)
    {
        return await _tokensRepository.SetNotActive(tokensDomain.Token, tokensDomain.RefreshToken);
    }
    
    private async Task<bool> TokensValid(string token, string refreshToken)
    {
        var tokensDb = await _tokensRepository.Get(token, refreshToken);

        if (tokensDb == null)
            return false;

        // check expire date of redresh token
        if (tokensDb.CreationDate.AddDays(7) < DateTime.Now)
            return false;

        if (!tokensDb.Active)
            return false;

        return true;
    }

    // get authed user
    public async Task<UserDomain?> GetCurrentUser(HttpContext context)
    {
        var tokens = GetTokens(context);

        if (tokens == null)
            return null;
        
        var claims = _tokenManager.GetPrincipalFromExpiredToken(tokens.Token);
        
        return await GetUser(claims);;
    }

    // get user from claims
    private async Task<UserDomain?> GetUser(ClaimsPrincipal claims)
    {
        var email = claims.Identity?.Name;

        if (string.IsNullOrEmpty(email))
            return null;

        var user = await _userRepository.Get(email);

        return UserDomainBuilder.Create(user);
    }
    
    // save tokens in db and return bool value saved or not
    private async Task<bool> SaveTokensAsync(HttpContext context, string token, string refreshToken, Guid userId)
    {
        var tokensDatabase = new TokensDatabase()
        {
            Token = token,
            RefreshToken = refreshToken,
            Ip = GetIpAddress(context.Request.Host.Host),
            Active = true,
            UserId = userId,
            CreationDate = DateTime.UtcNow
        };
        
        return await _tokensRepository.Create(tokensDatabase) > 0;
    }
    
    private async Task<bool> SaveTokensAsync(HttpContext context, Blank.User.Tokens tokensDomain, Guid userId)
    {
        var tokensDatabase = new TokensDatabase()
        {
            Token = tokensDomain.Token,
            RefreshToken = tokensDomain.RefreshToken,
            Ip = GetIpAddress(context.Request.Host.Host),
            Active = true,
            UserId = userId,
            CreationDate = DateTime.UtcNow
        };
        
        return await _tokensRepository.Create(tokensDatabase) > 0;
    }
    
    #region parsing

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
}