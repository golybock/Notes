using System.Security.Claims;
using Blank.User;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Services.Interfaces.User;

public interface IAuthService
{
    public Task<IActionResult> Login(LoginBlank loginBlank);
    
    public Task<IActionResult> Registration(UserBlank userBlank);

    // public Task<IActionResult> UpdatePassword(string newPassword, HttpContext context);
    
    public Task<IActionResult> UnLogin();
}