using System.Security.Claims;
using Blank.User;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Services.Interfaces.User;

public interface IAuthService
{
    public Task<IActionResult> Login(string email, string password, HttpContext context);
    
    public Task<IActionResult> Registration(UserBlank userBlank, HttpContext context);

    public Task<IActionResult> UpdatePassword(ClaimsPrincipal claimsPrincipal, string newPassword, HttpContext context);

    public Task<IActionResult> RefreshTokens(TokensBlank tokens, HttpContext context);
}