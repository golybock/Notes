using System.Security.Claims;
using Blank.User;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Services.Interfaces.User;

public interface IAuthService
{
    public Task<IActionResult> Login(string email, string password, HttpContext context);
    
    public Task<IActionResult> Registration(UserBlank userBlank, HttpContext context);

    public Task<IActionResult> UpdatePassword(IEnumerable<Claim> claims, string newPassword);

    public Task<IActionResult> RefreshTokens(TokensBlank tokens);
}