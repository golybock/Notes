using Blank.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Controllers.User;

public interface IUserController
{
    public Task<IActionResult> GetUser();

    public Task<IActionResult> UpdateUser(UserBlank userBlank);
}