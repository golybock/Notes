using Blank.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Auth;
using NotesApi.Services.Interfaces.Note;
using NotesApi.Services.Note;

namespace NotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NoteController : ControllerBase
{
    private readonly NoteService _noteService;
    private readonly AuthManager _authManager;
    
    public NoteController(IConfiguration configuration)
    {
        _noteService = new NoteService(configuration);
        _authManager = new AuthManager(configuration);
    }

    [HttpGet("Notes")]
    public async Task<IActionResult> Get()
    {
        var signed = await _authManager.IsSigned(HttpContext);

        if (signed == null)
            return new UnauthorizedResult();

        return await _noteService.Get(HttpContext, signed.Email);
    }

    [HttpGet("Note")]
    public async Task<IActionResult> Get(Guid guid)
    {
        var signed = await _authManager.IsSigned(HttpContext);

        if (signed == null)
            return new UnauthorizedResult();
        
        return await _noteService.Get(HttpContext, guid);
    }

    [HttpPost("Note")]
    public async Task<IActionResult> Create(NoteBlank blank)
    {
        return await _noteService.Create(HttpContext, blank);
    }

    [HttpPut("Note")]
    public async Task<IActionResult> Update(Guid guid, NoteBlank blank)
    {
        return await _noteService.Update(HttpContext, guid, blank);
    }
    
    [HttpDelete("Note")]
    public async Task<IActionResult> Delete(Guid guid)
    {
        return await _noteService.Delete(HttpContext, guid);
    }
    
    [HttpGet("SharedNotes")]
    public async Task<IActionResult> GetSharedNotes()
    {
        return await _noteService.GetShared(HttpContext);
    }
    
    [HttpPost("Share")]
    public async Task<IActionResult> Share(ShareBlank shareBlank)
    {
        return await _noteService.Share(HttpContext, shareBlank);
    }
    
    [HttpPut("UpdateShare")]
    public async Task<IActionResult> UpdateShare(ShareBlank shareBlank)
    {
        return await _noteService.UpdateShare(HttpContext, shareBlank);
    }
    
    [HttpDelete("DeleteShare")]
    public async Task<IActionResult> DeleteShare(Guid id, string email)
    {
        return await _noteService.DeleteShare(HttpContext, id, email);
    }
}