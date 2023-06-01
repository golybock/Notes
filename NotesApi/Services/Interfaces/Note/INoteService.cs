using System.Security.Claims;
using Blank.Note;
using Microsoft.AspNetCore.Mvc;
using Views.Note;

namespace NotesApi.Services.Interfaces.Note;

public interface INoteService
{
    public Task<IActionResult> Get();

    public Task<IActionResult> Get(Guid guid);

    public Task<IActionResult> Create(ClaimsPrincipal principal, NoteBlank noteBlank);

    public Task<IActionResult> Update(ClaimsPrincipal principal, Guid guid, NoteBlank noteBlank);

    // public Task<IActionResult> Share(ClaimsPrincipal claims, Guid guid, string email, int permissionLevel);
    //
    // public Task<IActionResult> DeleteShare(ClaimsPrincipal claims, Guid guid, string email, int permissionLevel);

    public Task<IActionResult> Delete(Guid guid);
}