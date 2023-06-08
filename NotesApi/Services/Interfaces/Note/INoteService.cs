using System.Security.Claims;
using Blank.Note;
using Domain.User;
using Microsoft.AspNetCore.Mvc;
using Views.Note;

namespace NotesApi.Services.Interfaces.Note;

public interface INoteService
{
    public Task<IActionResult> Get(UserDomain user);

    public Task<IActionResult> Get(UserDomain user, Guid id);

    public Task<IActionResult> Create(UserDomain user, NoteBlank noteBlank);

    public Task<IActionResult> Update(UserDomain user, Guid id, NoteBlank noteBlank);

    public Task<IActionResult> Share(UserDomain user, ShareBlank shareBlank);

    public Task<IActionResult> UpdateShare(UserDomain user, ShareBlank shareBlank);

    public Task<IActionResult> DeleteShare(UserDomain user, Guid id, string email);

    public Task<IActionResult> Delete(UserDomain user, Guid id);
}