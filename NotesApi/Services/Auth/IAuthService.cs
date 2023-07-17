using Blank.User;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Services.Auth;

public interface IAuthService
{
    public Task<IActionResult> SignIn(HttpContext context, LoginBlank loginBlank);
    
    public Task<IActionResult> SignUp(HttpContext context, UserBlank userBlank);

    public Task<IActionResult> SignOut(HttpContext context);
}