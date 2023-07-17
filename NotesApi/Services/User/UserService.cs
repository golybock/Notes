using System.Security.Claims;
using Blank.User;
using DatabaseBuilder.User;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.User.UserManager;
using Repositories.Repositories.User;
using ViewBuilder.User;

namespace NotesApi.Services.User;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserManager _userManager;

    public UserService(IUserRepository userRepository, IUserManager userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }
    
    public async Task<IActionResult> Get(ClaimsPrincipal claims)
    {
        var user = await _userManager.Get(claims);
        
        if (user == null)
            return new BadRequestObjectResult("Invalid token claims, please login again");
        
        var userView = UserViewBuilder.Create(user);
        
        return new OkObjectResult(userView);
    }
    
    public async Task<IActionResult> Update(ClaimsPrincipal claims, UserBlank userBlank)
    {
        var user = await _userManager.Get(claims);

        if (user == null)
            return new BadRequestObjectResult("Invalid token claims, please login again");
        
        var userDatabase = UserDatabaseBuilder.Create(userBlank);

        var updated = await _userRepository.Update(user.Id, userDatabase);

        if (!updated)
            return new BadRequestResult();

        return new OkResult();
    }

}