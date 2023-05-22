using Blank.Note;
using Microsoft.AspNetCore.Mvc;
using Views.Note;

namespace NotesApi.Services.Interfaces.Note;

public interface INoteService
{
    public Task<IActionResult> Get();
    
    public Task<IActionResult> Get(int id);

    public Task<IActionResult> Create(NoteBlank noteBlank);

    public Task<IActionResult> Update(int id, NoteBlank blank);

    public Task<IActionResult> Delete(int id);
}