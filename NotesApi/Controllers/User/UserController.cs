using Blank.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.User;

namespace NotesApi.Controllers.User;

[ApiController, Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(IConfiguration configuration)
    {
        _userService = new UserService(configuration);
    }

    [HttpGet("User")]
    public async Task<IActionResult> GetUser()
    {
        return await _userService.Get(User);
    }
    
    // [HttpPut("User")]
    // public async Task<IActionResult> UpdateUser(UserBlank userBlank)
    // {
    //     return await _userService.Update(HttpContext, userBlank);
    // }
}