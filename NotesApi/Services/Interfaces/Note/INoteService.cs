using System.Security.Claims;
using Blank.Note;
using Microsoft.AspNetCore.Mvc;
using Views.Note;

namespace NotesApi.Services.Interfaces.Note;

public interface INoteService
{
    public Task<IActionResult> Get(ClaimsPrincipal claims);

    public Task<IActionResult> Get(ClaimsPrincipal claims, Guid id);

    public Task<IActionResult> Create(ClaimsPrincipal claims, NoteBlank noteBlank);

    public Task<IActionResult> Update(ClaimsPrincipal claims, Guid id, NoteBlank noteBlank);

    public Task<IActionResult> Share(ClaimsPrincipal claims, ShareBlank shareBlank);

    public Task<IActionResult> UpdateShare(ClaimsPrincipal claims, ShareBlank shareBlank);

    public Task<IActionResult> DeleteShare(ClaimsPrincipal claims, Guid id, string email);

    public Task<IActionResult> Delete(ClaimsPrincipal claims, Guid id);
}