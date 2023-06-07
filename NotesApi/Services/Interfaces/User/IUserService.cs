using System.Security.Claims;
using Blank.User;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Services.Interfaces.User;

public interface IUserService
{
    public Task<IActionResult> Get(HttpContext context);
    public Task<IActionResult> Update(HttpContext context, UserBlank userBlank);
}