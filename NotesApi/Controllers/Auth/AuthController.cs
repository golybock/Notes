using Blank.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.Auth;

namespace NotesApi.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase 
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

    [Authorize]
    [HttpPost("UpdatePassword")]
    public async Task<IActionResult> UpdatePassword(string newPassword)
    {
        return await _authService.UpdatePassword(User, newPassword, HttpContext);
    }

    // [HttpPost("RefreshTokens")]
    // public async Task<IActionResult> RefreshTokens(TokensBlank tokens)
    // {
    //     return await _authService.RefreshTokens(tokens, HttpContext);
    // }
}