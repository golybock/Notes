using Blank.Note;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.Note;

namespace NotesApi.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;

    public NoteController(INoteService noteService)
    {
        _noteService = noteService;
    }

    [HttpGet("Notes")]
    public async Task<IActionResult> Get()
    {
        return await _noteService.Get(User);
    }

    [HttpGet("Note")]
    public async Task<IActionResult> Get(Guid id)
    {
        return await _noteService.Get(User, id);
    }

    [HttpPost("Note")]
    public async Task<IActionResult> Create(NoteBlank blank)
    {
        return await _noteService.Create(User, blank);
    }

    [HttpPut("Note")]
    public async Task<IActionResult> Update(Guid id, NoteBlank blank)
    {
        return await _noteService.Update(User, id, blank);
    }
    
    [HttpDelete("Note")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return await _noteService.Delete(User, id);
    }
    
    [HttpPost("Image")]
    public async Task<IActionResult> UploadImage(Guid noteId)
    {
        // select file from request
        var file = Request.Form.Files.FirstOrDefault();

        if (file == null)
            return BadRequest();
        
        return await _noteService.UploadImage(file, noteId);
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