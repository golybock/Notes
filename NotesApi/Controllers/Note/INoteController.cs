using Blank.Note;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Controllers.Note;

public interface INoteController
{
    public Task<IActionResult> Get();

    public Task<IActionResult> Get(Guid id);

    public Task<IActionResult> Create(NoteBlank blank);

    public Task<IActionResult> Update(Guid id, NoteBlank blank);

    public Task<IActionResult> Delete(Guid id);

    public Task<IActionResult> UploadImage(Guid noteId);

    public Task<IActionResult> GetSharedNotes();

    public Task<IActionResult> Share(ShareBlank shareBlank);
    
    public Task<IActionResult> UpdateShare(ShareBlank shareBlank);

    public Task<IActionResult> DeleteShare(Guid id, string email);
}