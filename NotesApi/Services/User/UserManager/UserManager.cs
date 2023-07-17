using System.Security.Claims;
using Blank.User;
using DatabaseBuilder.User;
using Domain.User;
using DomainBuilder.User;
using Repositories.Repositories.User;

namespace NotesApi.Services.User.UserManager;

public class UserManager : IUserManager
{
    private readonly IUserRepository _userRepository;

    public UserManager(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<UserDomain?> Get(string email)
    {
        var user = await _userRepository.Get(email);

        if (user == null)
            return null;

        return UserDomainBuilder.Create(user);
    }

    public async Task<UserDomain?> Get(Guid id)
    {
        var user = await _userRepository.Get(id);

        if (user == null)
            return null;

        return UserDomainBuilder.Create(user);
    }

    public async Task<UserDomain?> Get(ClaimsPrincipal claimsPrincipal)
    {
        var id = claimsPrincipal.FindFirst(ClaimTypes.Authentication)?.Value;

        if (id == null)
            return null;
        
        var guid = Guid.Parse(id);
        
        var user = await _userRepository.Get(guid);
    
        return UserDomainBuilder.Create(user!);
    }

    public async Task<Guid> Create(UserBlank userBlank, string hashedPassword)
    {
        var id = Guid.NewGuid();

        var user = UserDatabaseBuilder.Create(id, userBlank, hashedPassword);

        return await _userRepository.Create(user);
    }

    public async Task Update(Guid id, UserBlank userBlank)
    {
        var userDatabase = UserDatabaseBuilder.Create(userBlank);

        var updated = await _userRepository.Update(id, userDatabase);

        if (!updated)
            throw new Exception("User not found");
    }
}