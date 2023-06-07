using Blank.User;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.Auth;

namespace NotesApi.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase, IAuthController
{
    private readonly AuthService _authService;

    public AuthController(IConfiguration configuration)
    {
        _authService = new AuthService(configuration);
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> SignIn(LoginBlank loginBlank)
    {
        return await _authService.SignIn(HttpContext ,loginBlank);
    }
    
    [HttpPost("Registration")]
    public async Task<IActionResult> SignUp(UserBlank userBlank)
    {
        return await _authService.SignUp(HttpContext ,userBlank);
    }

    [HttpPost("UnLogin")]
    public new async Task<IActionResult> SignOut()
    {
        return await _authService.SignOut(HttpContext);
    }

    // [HttpPost("UpdatePassword")]
    // public async Task<IActionResult> UpdatePassword(string newPassword)
    // {
    //     return await _authService.UpdatePassword(newPassword, HttpContext);
    // }
}