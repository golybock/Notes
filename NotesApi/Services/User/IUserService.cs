using System.Security.Claims;
using Blank.User;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Services.User;

public interface IUserService
{
    public Task<IActionResult> Get(ClaimsPrincipal claims);
    public Task<IActionResult> Update(ClaimsPrincipal claims, UserBlank userBlank);
}