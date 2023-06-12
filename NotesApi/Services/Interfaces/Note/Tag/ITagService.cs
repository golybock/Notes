using Blank.Note.Tag;
using Microsoft.AspNetCore.Mvc;
using Views.Note.Tag;

namespace NotesApi.Services.Interfaces.Note.Tag;

public interface ITagService
{
    public Task<IActionResult> Get();
    
    public Task<IActionResult> Get(Guid id);

    public Task<IActionResult> GetByNote(Guid noteId);

    public Task<IActionResult> Create(TagBlank tagBlank);
    
    // public Task<IActionResult> Update(int id, TagBlank tagBlank);
    
    public Task<IActionResult> Delete(int id);
}