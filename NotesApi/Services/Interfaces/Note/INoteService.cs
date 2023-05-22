using Blank.Note;
using Microsoft.AspNetCore.Mvc;
using Views.Note;

namespace NotesApi.Services.Interfaces.Note;

public interface INoteService
{
    public Task<IActionResult> Get();

    public Task<IActionResult> Get(Guid guid);

    public Task<IActionResult> Create(NoteBlank noteBlank);

    public Task<IActionResult> Update(Guid guid, NoteBlank blank);

    public Task<IActionResult> Delete(Guid guid);
}