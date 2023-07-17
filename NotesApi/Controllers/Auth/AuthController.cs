using Blank.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.Auth;

namespace NotesApi.Controllers.Auth;

[ApiController, AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : ControllerBase, IAuthController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn(LoginBlank loginBlank)
    {
        return await _authService.SignIn(HttpContext ,loginBlank);
    }
    
    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp(UserBlank userBlank)
    {
        return await _authService.SignUp(HttpContext ,userBlank);
    }

    [HttpPost("SignOut")]
    public new async Task<IActionResult> SignOut()
    {
        return await _authService.SignOut(HttpContext);
    }
}