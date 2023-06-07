using Blank.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Auth;
using NotesApi.Services.Interfaces.Note;
using NotesApi.Services.Note;

namespace NotesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
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

        if (!signed)
            return new UnauthorizedResult();

        var user = await _authManager.GetCurrentUser(HttpContext);
        
        return await _noteService.Get(user.Email);
    }

    [HttpGet("Note")]
    public async Task<IActionResult> Get(Guid guid)
    {
        return await _noteService.Get(User, guid);
    }

    [HttpPost("Note")]
    public async Task<IActionResult> Create(NoteBlank blank)
    {
        return await _noteService.Create(User, blank);
    }

    [HttpPut("Note")]
    public async Task<IActionResult> Update(Guid guid, NoteBlank blank)
    {
        return await _noteService.Update(User, guid, blank);
    }
    
    [HttpDelete("Note")]
    public async Task<IActionResult> Delete(Guid guid)
    {
        return await _noteService.Delete(User, guid);
    }
    
    [HttpGet("SharedNotes")]
    public async Task<IActionResult> GetSharedNotes()
    {
        return await _noteService.GetShared(User);
    }
    
    [HttpPost("Share")]
    public async Task<IActionResult> Share(ShareBlank shareBlank)
    {
        return await _noteService.Share(User, shareBlank);
    }
    
    [HttpPut("UpdateShare")]
    public async Task<IActionResult> UpdateShare(ShareBlank shareBlank)
    {
        return await _noteService.UpdateShare(User, shareBlank);
    }
    
    [HttpDelete("DeleteShare")]
    public async Task<IActionResult> DeleteShare(Guid id, string email)
    {
        return await _noteService.DeleteShare(User, id, email);
    }
}