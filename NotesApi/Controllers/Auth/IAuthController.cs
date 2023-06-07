using Blank.User;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Controllers.Auth;

public interface IAuthController
{
    public Task<IActionResult> SignIn(LoginBlank loginBlank);

    public Task<IActionResult> SignUp(UserBlank userBlank);

    // public Task<IActionResult> UpdatePassword(string newPassword);

    public Task<IActionResult> SignOut();
}