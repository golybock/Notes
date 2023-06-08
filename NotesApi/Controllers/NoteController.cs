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

        return await _noteService.Get(signed);
    }

    [HttpGet("Note")]
    public async Task<IActionResult> Get(Guid guid)
    {
        var signed = await _authManager.IsSigned(HttpContext);

        if (signed == null)
            return new UnauthorizedResult();
        
        return await _noteService.Get(signed, guid);
    }

    [HttpPost("Note")]
    public async Task<IActionResult> Create(NoteBlank blank)
    {
        var signed = await _authManager.IsSigned(HttpContext);

        if (signed == null)
            return new UnauthorizedResult();
    
        return await _noteService.Create(signed, blank);
    }

    [HttpPut("Note")]
    public async Task<IActionResult> Update(Guid guid, NoteBlank blank)
    {
        var signed = await _authManager.IsSigned(HttpContext);

        if (signed == null)
            return new UnauthorizedResult();
        
        return await _noteService.Update(signed, guid, blank);
    }
    
    [HttpDelete("Note")]
    public async Task<IActionResult> Delete(Guid guid)
    {
        var signed = await _authManager.IsSigned(HttpContext);

        if (signed == null)
            return new UnauthorizedResult();
        
        return await _noteService.Delete(signed, guid);
    }
    
    [HttpGet("SharedNotes")]
    public async Task<IActionResult> GetSharedNotes()
    {
        var signed = await _authManager.IsSigned(HttpContext);

        if (signed == null)
            return new UnauthorizedResult();
        
        return await _noteService.GetShared(signed);
    }
    
    [HttpPost("Share")]
    public async Task<IActionResult> Share(ShareBlank shareBlank)
    {
        var signed = await _authManager.IsSigned(HttpContext);

        if (signed == null)
            return new UnauthorizedResult();
        
        return await _noteService.Share(signed, shareBlank);
    }
    
    [HttpPut("UpdateShare")]
    public async Task<IActionResult> UpdateShare(ShareBlank shareBlank)
    {
        var signed = await _authManager.IsSigned(HttpContext);

        if (signed == null)
            return new UnauthorizedResult();
        
        return await _noteService.UpdateShare(signed, shareBlank);
    }
    
    [HttpDelete("DeleteShare")]
    public async Task<IActionResult> DeleteShare(Guid id, string email)
    {
        var signed = await _authManager.IsSigned(HttpContext);

        if (signed == null)
            return new UnauthorizedResult();
        
        return await _noteService.DeleteShare(signed, id, email);
    }
}