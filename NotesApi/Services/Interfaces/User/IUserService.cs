using System.Security.Claims;
using Blank.User;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Services.Interfaces.User;

public interface IUserService
{
    public Task<IActionResult> Get(ClaimsPrincipal claimsPrincipal);
    public Task<IActionResult> Update(ClaimsPrincipal claimsPrincipal, NoteUserBlank noteUserBlank);
}