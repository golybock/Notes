using System.Security.Claims;
using Blank.User;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Services.Interfaces.User;

public interface IUserService
{
    public Task<IActionResult> Get(ClaimsPrincipal claims);
    public Task<IActionResult> Update(ClaimsPrincipal claims, UserBlank userBlank);
}