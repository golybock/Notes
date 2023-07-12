using Blank.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.Interfaces.User;
using NotesApi.Services.User;

namespace NotesApi.Controllers.User;

[ApiController, Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("User")]
    public async Task<IActionResult> GetUser()
    {
        return await _userService.Get(User);
    }
}