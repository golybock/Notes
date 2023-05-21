using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.Note;

namespace NotesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NoteController : ControllerBase
{
    private readonly NoteService _noteService;

    public NoteController(IConfiguration configuration)
    {
        _noteService = new NoteService(configuration);
    }

    [HttpGet("Notes")]
    public async Task<IActionResult> Get()
    {
        return await _noteService.Get();
    }
    
    [HttpGet("Note")]
    public async Task<IActionResult> Get(int id)
    {
        return await _noteService.Get(id);
    }
}