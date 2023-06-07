using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Database.User;
using Domain.User;
using DomainBuilder.User;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Auth.Cookie;
using Repositories.Repositories.User;

namespace NotesApi.Auth;

public class AuthManager
{
    private readonly IConfiguration _configuration;
    private readonly CookieManager _cookieManager;
    private readonly TokenManager _tokenManager;

    private readonly TokensRepository _tokensRepository;
    private readonly NoteUserRepository _noteUserRepository;

    public AuthManager(IConfiguration configuration)
    {
        _configuration = configuration;

        _noteUserRepository = new NoteUserRepository(configuration);
        _cookieManager = new CookieManager(configuration);
        _tokenManager = new TokenManager(configuration);
        _tokensRepository = new TokensRepository(configuration);
    }

    public async Task SignInAsync(HttpContext context, string email)
    {
        var claims = _tokenManager.CreateIdentityClaims(email);

        var decodedToken = _tokenManager.GenerateJwtSecurityToken(claims);
        var refreshToken = _tokenManager.GenerateRefreshToken();

        string token = new JwtSecurityTokenHandler().WriteToken(decodedToken);

        var tokens = new TokensDomain()
        {
            Token = token,
            RefreshToken = refreshToken
        };

        var tokensDatabase = new TokensDatabase()
        {
            Token = token,
            RefreshToken = refreshToken,
            Ip = GetIpAddress(context.Request.Host.Host),
            Active = true,
            UserId = (await GetUser(email)).Id,
            CreationDate = DateTime.UtcNow
        };
        
        await _tokensRepository.Create(tokensDatabase);

        _cookieManager.SetTokens(context, tokens);
    }
    
    private async Task<UserDomain?> GetUser(string email)
    {
        var user = await _noteUserRepository.Get(email);

        return UserDomainBuilder.Create(user);
    }

    /// <summary>
    /// Check tokens and refresh it if needed
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<bool> IsSigned(HttpContext context)
    {
        TokensDomain tokens;
        
        try
        {
            tokens = _cookieManager.GetTokens(context);
        }
        catch (Exception e)
        {
            return false;
        }

        // try refresh tokens
        if (_tokenManager.TokenExpired(tokens.Token))
        {
            var claims = _tokenManager.GetPrincipalFromExpiredToken(tokens.Token);

            var user = await GetUser(claims);

            if (user == null)
                return false;

            var newClaims = _tokenManager.CreateIdentityClaims(user.Email);

            var decodedToken = _tokenManager.GenerateJwtSecurityToken(newClaims);
            var refreshToken = _tokenManager.GenerateRefreshToken();

            string token = new JwtSecurityTokenHandler().WriteToken(decodedToken);

            var newTokens = new TokensDomain()
            {
                Token = token,
                RefreshToken = refreshToken
            };
            
            #region tokens db and their validation

            var tokensDb = await _tokensRepository.Get(token, refreshToken);

            if (tokensDb == null)
                return false;

            // check expire date
            if (tokensDb.CreationDate.AddDays(7) < DateTime.Now)
                return false;

            if (!tokensDb.Active)
                return false;
            
            var tokensDatabase = new TokensDatabase()
            {
                Token = token,
                RefreshToken = refreshToken,
                Ip = GetIpAddress(context.Request.Host.Host),
                Active = true,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow
            };
        
            await _tokensRepository.Create(tokensDatabase);
            
            #endregion

            _cookieManager.SetTokens(context, newTokens);

            return true;
        }

        try
        {
            var es = _tokenManager.GetPrincipalFromToken(tokens.Token);
            return true;
        }
        catch(Exception e)
        {
            return false;    
        }
    }

    public void Logout(HttpContext context)
    {
        _cookieManager.DeleteTokens(context);
    }

    public async Task<UserDomain?> GetCurrentUser(HttpContext context)
    {
        var tokens = _cookieManager.GetTokens(context);

        var claims = _tokenManager.GetPrincipalFromToken(tokens.Token);

        var user = await GetUser(claims);

        return user;
    }

    private async Task<UserDomain?> GetUser(ClaimsPrincipal claims)
    {
        var email = claims.Identity?.Name;

        if (string.IsNullOrEmpty(email))
            return null;

        var user = await _noteUserRepository.Get(email);

        return UserDomainBuilder.Create(user);
    }
    
    private async Task<bool> SaveTokens(TokensDatabase tokensDatabase)
    {
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