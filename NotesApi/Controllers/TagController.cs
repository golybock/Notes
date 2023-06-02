using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blank.Note.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.Note.Tag;

namespace NotesApi.Controllers;

[Route("api/[controller]")]
[ApiController, Authorize]
public class TagController : ControllerBase
{
    private readonly TagService _tagService;

    public TagController(IConfiguration configuration)
    {
        _tagService = new TagService(configuration);
    }

    [HttpGet("Tags")]
    public async Task<IActionResult> Get()
    {
        return await _tagService.Get();
    }
    
    [HttpGet("Tag")]
    public async Task<IActionResult> Get(int id)
    {
        return await _tagService.Get(id);
    }
    
    // не нужно(подгружается вместе с заметкой)
    [HttpGet("NoteTags"), Authorize]
    public async Task<IActionResult> GetByNote(Guid noteId)
    {
        return await _tagService.GetByNote(noteId);
    }

    [HttpPost("Tag")]
    public async Task<IActionResult> Create(TagBlank tagBlank)
    {
        return await _tagService.Create(tagBlank);
    }
    
    [HttpPut("Tag")]
    public async Task<IActionResult> Update(int id, TagBlank tagBlank)
    {
        return await _tagService.Update(id, tagBlank);
    }
    
    [HttpDelete("Tag")]
    public async Task<IActionResult> Delete(int id)
    {
        return await _tagService.Delete(id);
    }
}