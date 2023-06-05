using Blank.User;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Controllers.Auth;

public interface IAuthController
{
    public Task<IActionResult> Login(LoginBlank loginBlank);

    public Task<IActionResult> Registration(UserBlank userBlank);

    public Task<IActionResult> UpdatePassword(string newPassword);

    // public async Task<IActionResult> RefreshTokens(TokensBlank tokens);
}