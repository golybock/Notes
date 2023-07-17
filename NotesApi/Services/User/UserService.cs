using System.Security.Claims;
using Blank.User;
using DatabaseBuilder.User;
using Microsoft.AspNetCore.Mvc;
using NotesApi.RefreshCookieAuthScheme.AuthManager;
using Repositories.Repositories.User;
using ViewBuilder.User;

namespace NotesApi.Services.User;

public class UserService : IUserService
{
    private readonly UserRepository _userRepository;
    private readonly IAuthManager _authManager;
    private readonly UserManager _userManager;

    public UserService(IAuthManager authManager, UserManager userManager, UserRepository userRepository)
    {
        _authManager = authManager;
        _userManager = userManager;
        _userRepository = userRepository;
    }
    
    public async Task<IActionResult> Get(ClaimsPrincipal claims)
    {
        var user = await _userManager.GetUser(claims);
        
        var userView = UserViewBuilder.Create(user);
        
        return new OkObjectResult(userView);
    }
    
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