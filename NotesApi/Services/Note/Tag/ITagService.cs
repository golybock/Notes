using Blank.Note.Tag;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Services.Note.Tag;

public interface ITagService
{
    public Task<IActionResult> Get();
    
    public Task<IActionResult> Get(Guid id);

    public Task<IActionResult> Create(TagBlank tagBlank);

    public Task<IActionResult> Delete(Guid id);
}