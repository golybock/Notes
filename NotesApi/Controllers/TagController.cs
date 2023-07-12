using Blank.Note.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.Interfaces.Note.Tag;
using NotesApi.Services.Note.Tag;

namespace NotesApi.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet("Tags")]
    public async Task<IActionResult> Get()
    {
        return await _tagService.Get();
    }
    
    [HttpGet("Tag")]
    public async Task<IActionResult> Get(Guid id)
    {
        return await _tagService.Get(id);
    }

    [HttpPost("Tag")]
    public async Task<IActionResult> Create(TagBlank tagBlank)
    {
        return await _tagService.Create(tagBlank);
    }

    [HttpDelete("Tag")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _tagService.Delete(id);
    }
}