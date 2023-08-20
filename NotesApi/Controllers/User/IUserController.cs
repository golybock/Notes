using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Controllers.User;

public interface IUserController
{
    public Task<IActionResult> GetUser();
}