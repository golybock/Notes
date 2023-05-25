using Blank.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.Interfaces.Note;
using NotesApi.Services.Note;

namespace NotesApi.Controllers;

[Route("api/[controller]")]
[ApiController, Authorize]
public class NoteController : ControllerBase, INoteService
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
    public async Task<IActionResult> Get(Guid guid)
    {
        return await _noteService.Get(guid);
    }

    [HttpPost("Note")]
    public async Task<IActionResult> Create(NoteBlank blank)
    {
        return await _noteService.Create(blank);
    }

    [HttpPut("Note")]
    public async Task<IActionResult> Update(Guid guid, NoteBlank blank)
    {
        return await _noteService.Update(guid, blank);
    }
    
    [HttpDelete("Note")]
    public async Task<IActionResult> Delete(Guid guid)
    {
        return await _noteService.Delete(guid);
    }
}