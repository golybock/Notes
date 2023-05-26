using Blank.User;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Services.Interfaces.User;

public interface IUserService
{
    public Task<IActionResult> Get(string email);
    public Task<IActionResult> Update(int id, NoteUserBlank noteUserBlank);
}