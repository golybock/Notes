using System.Security.Claims;
using Blank.User;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Services.Interfaces.User;

public interface IAuthService
{
    public Task<IActionResult> Login(LoginBlank loginBlank, HttpContext context);
    
    public Task<IActionResult> Registration(UserBlank userBlank, HttpContext context);

    public Task<IActionResult> UpdatePassword(string newPassword, HttpContext context);
}