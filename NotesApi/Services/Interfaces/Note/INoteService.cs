using System.Security.Claims;
using Blank.Note;
using Microsoft.AspNetCore.Mvc;
using Views.Note;

namespace NotesApi.Services.Interfaces.Note;

public interface INoteService
{
    public Task<IActionResult> Get(HttpContext context, string email);

    public Task<IActionResult> Get(HttpContext context, Guid id);

    public Task<IActionResult> Create(HttpContext context, NoteBlank noteBlank);

    public Task<IActionResult> Update(HttpContext context, Guid id, NoteBlank noteBlank);

    public Task<IActionResult> Share(HttpContext context, ShareBlank shareBlank);

    public Task<IActionResult> UpdateShare(HttpContext context, ShareBlank shareBlank);

    public Task<IActionResult> DeleteShare(HttpContext context, Guid id, string email);

    public Task<IActionResult> Delete(HttpContext context, Guid id);
}