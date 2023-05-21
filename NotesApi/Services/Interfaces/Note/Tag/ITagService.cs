using Microsoft.AspNetCore.Mvc;
using Views.Note.Tag;

namespace NotesApi.Services.Interfaces.Note.Tag;

public interface ITagService
{
    public Task<IActionResult> Get();
    
    public Task<IActionResult> Get(int id);

    public Task<IActionResult> GetByNote(int noteId);
}