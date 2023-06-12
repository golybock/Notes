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

[ApiController]
[Route("api/[controller]")]
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