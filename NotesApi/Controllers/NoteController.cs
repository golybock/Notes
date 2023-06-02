using Blank.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.Interfaces.Note;
using NotesApi.Services.Note;

namespace NotesApi.Controllers;

[Route("api/[controller]")]
[ApiController, Authorize]
public class NoteController : ControllerBase
{
    private readonly NoteService _noteService;

    public NoteController(IConfiguration configuration)
    {
        _noteService = new NoteService(configuration);
    }

    [HttpGet("Notes"), Authorize]
    public async Task<IActionResult> Get()
    {
        return await _noteService.Get(User);
    }

    [HttpGet("Note"), Authorize]
    public async Task<IActionResult> Get(Guid guid)
    {
        return await _noteService.Get(User, guid);
    }

    [HttpPost("Note"), Authorize]
    public async Task<IActionResult> Create(NoteBlank blank)
    {
        return await _noteService.Create(User, blank);
    }

    [HttpPut("Note"), Authorize]
    public async Task<IActionResult> Update(Guid guid, NoteBlank blank)
    {
        return await _noteService.Update(User, guid, blank);
    }
    
    [HttpDelete("Note"),Authorize]
    public async Task<IActionResult> Delete(Guid guid)
    {
        return await _noteService.Delete(User, guid);
    }
}