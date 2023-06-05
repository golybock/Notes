using Blank.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.User;

namespace NotesApi.Controllers.User;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase, IUserController
{
    private readonly UserService _userService;

    public UserController(IConfiguration configuration)
    {
        _userService = new UserService(configuration);
    }

    [HttpGet("User"), Authorize]
    public async Task<IActionResult> GetUser()
    {
        return await _userService.Get(User);
    }
    
    [HttpPut("User"), Authorize]
    public async Task<IActionResult> UpdateUser(UserBlank userBlank)
    {
        return await _userService.Update(User, userBlank);
    }
}