using Blank.User;
using Microsoft.AspNetCore.Mvc;

namespace NotesApi.Services.Interfaces.User;

public interface IUserService
{
    public Task<IActionResult> Get(int id);
    
    public Task<IActionResult> Get(string email);

    public Task<IActionResult> Create(UserBlank userBlank);

    public Task<IActionResult> Update(int id, UserBlank userBlank);
    
    public Task<IActionResult> Delete(int id);
}