using System.Security.Claims;
using Blank.User;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Services.Interfaces.User;

public interface IAuthService
{
    public Task<IActionResult> SignIn(LoginBlank loginBlank);
    
    public Task<IActionResult> SignUp(UserBlank userBlank);

    // public Task<IActionResult> UpdatePassword(string newPassword, HttpContext context);
    
    public Task<IActionResult> SignOut();
}