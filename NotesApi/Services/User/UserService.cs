using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Blank.User;
using DatabaseBuilder.User;
using Domain.User;
using DomainBuilder.User;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.Interfaces.User;
using Repositories.Repositories.User;
using ViewBuilder.User;

namespace NotesApi.Services.User;

public class UserService : IUserService
{
    private readonly UserRepository _userRepository;

    public UserService(IConfiguration configuration)
    {
        _userRepository = new UserRepository(configuration);
    }

    public async Task<IActionResult> Get(ClaimsPrincipal claimsPrincipal)
    {
        var user = await GetUser(claimsPrincipal);

        if (user == null)
            return new UnauthorizedResult();

        var userView = UserViewBuilder.Create(user);
        
        return new OkObjectResult(userView);
    }

    public async Task<IActionResult> Update(ClaimsPrincipal claimsPrincipal, UserBlank userBlank)
    {
        var user = await GetUser(claimsPrincipal);

        if (user == null)
            return new UnauthorizedResult();
        
        var userDatabase = UserDatabaseBuilder.Create(userBlank);

        var updated = await _userRepository.Update(user.Email, userDatabase);

        if (!updated)
            return new BadRequestResult();

        return new OkResult();
    }

    private async Task<UserDomain?> GetUser(ClaimsPrincipal claims)
    {
        var email = claims.Identity?.Name;

        if (string.IsNullOrEmpty(email))
            return null;

        var user = await _userRepository.Get(email);

        return UserDomainBuilder.Create(user);
    }
}