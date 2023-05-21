using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.Note.Tag;

namespace NotesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
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
    
    [HttpGet("NoteTags")]
    public async Task<IActionResult> GetByNote(int noteId)
    {
        return await _tagService.GetByNote(noteId);
    }
}