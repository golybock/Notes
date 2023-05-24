using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blank.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.Interfaces.User;
using NotesApi.Services.User;

namespace NotesApi.Controllers;

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
    public async Task<IActionResult> Login(string email, string password)
    {
        return await _authService.Login(email, password, HttpContext);
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

    [HttpPost("RefreshTokens")]
    public async Task<IActionResult> RefreshTokens(TokensBlank tokens)
    {
        return await _authService.RefreshTokens(tokens, HttpContext);
    }
}