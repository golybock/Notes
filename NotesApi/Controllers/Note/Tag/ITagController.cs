using Blank.Note.Tag;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Controllers.Note.Tag;

public interface ITagController
{
    public Task<IActionResult> Get();

    public Task<IActionResult> Get(Guid id);

    public Task<IActionResult> Create(TagBlank tagBlank);

    public Task<IActionResult> Delete(Guid id);
}