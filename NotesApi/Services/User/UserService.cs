using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Blank.User;
using DatabaseBuilder.User;
using Domain.User;
using DomainBuilder.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.RefreshCookieAuthScheme;
using NotesApi.RefreshCookieAuthScheme.AuthManager;
using NotesApi.Services.Interfaces.User;
using Repositories.Repositories.User;
using ViewBuilder.User;

namespace NotesApi.Services.User;

public class UserService : IUserService
{
    private readonly UserRepository _userRepository;
    private readonly AuthManager _authManager;
    private readonly UserManager _userManager;

    public UserService(IConfiguration configuration)
    {
        _authManager = new AuthManager(configuration);
        _userManager = new UserManager(configuration);
        _userRepository = new UserRepository(configuration);
    }

    [Authorize]
    public async Task<IActionResult> Get(ClaimsPrincipal claims)
    {
        var user = await _userManager.GetUser(claims);
        
        var userView = UserViewBuilder.Create(user);
        
        return new OkObjectResult(userView);
    }

    [Authorize]
    public async Task<IActionResult> Update(ClaimsPrincipal claims, UserBlank userBlank)
    {
        var user = await _userManager.GetUser(claims);
        
        var userDatabase = UserDatabaseBuilder.Create(userBlank);

        var updated = await _userRepository.Update(user.Id, userDatabase);

        if (!updated)
            return new BadRequestResult();

        return new OkResult();
    }

}