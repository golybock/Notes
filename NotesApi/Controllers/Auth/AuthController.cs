using Blank.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.Auth;

namespace NotesApi.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase, IAuthController
{
    private readonly AuthService _authService;

    public AuthController(IConfiguration configuration)
    {
        _authService = new AuthService(configuration);
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginBlank loginBlank)
    {
        return await _authService.Login(loginBlank, HttpContext);
    }
    
    [HttpPost("Registration")]
    public async Task<IActionResult> Registration(UserBlank userBlank)
    {
        return await _authService.Registration(userBlank, HttpContext);
    }

    [HttpPost("UnLogin")]
    public async Task<IActionResult> UnLogin()
    {
        throw new NotImplementedException();
    }

    // [HttpPost("UpdatePassword")]
    // public async Task<IActionResult> UpdatePassword(string newPassword)
    // {
    //     return await _authService.UpdatePassword(newPassword, HttpContext);
    // }
}