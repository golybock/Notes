using System.Security.Claims;
using Domain.User;
using DomainBuilder.User;
using NotesApi.RefreshCookieAuthScheme;
using Repositories.Repositories.Interfaces.User;
using Repositories.Repositories.User;

namespace NotesApi.Services.User;

public class UserManager
{
    private readonly IUserRepository _userRepository;

    public UserManager(IConfiguration configuration)
    {
        _userRepository = new UserRepository(configuration);
    }

    public UserManager(RefreshCookieOptions options)
    {
        _userRepository = new UserRepository(options.ConnectionString);
    }
    
    public async Task<UserDomain> GetUser(ClaimsPrincipal claims)
    {
        var id = claims.FindFirst(ClaimTypes.Authentication)?.Value;
        
        Guid guid = Guid.Parse(id!);
        
        var user = await _userRepository.Get(guid);

        return UserDomainBuilder.Create(user!);
    }
    
    public async Task<UserDomain?> GetUser(string email)
    {
        var user = await _userRepository.Get(email);

        if (user == null)
            return null;

        return UserDomainBuilder.Create(user);
    }

    public async Task<UserDomain?> GetUser(Guid id)
    {
        var user = await _userRepository.Get(id);

        if (user == null)
            return null;

        return UserDomainBuilder.Create(user);
    }
}