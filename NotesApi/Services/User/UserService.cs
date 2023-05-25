using Blank.User;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Repositories.User;
using NotesApi.Services.Interfaces.User;

namespace NotesApi.Services.User;

public class UserService : IUserService
{
    private readonly NoteUserRepository _noteUserRepository;

    public UserService(IConfiguration configuration)
    {
        _noteUserRepository = new NoteUserRepository(configuration);
    }

    public async Task<IActionResult> Get(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> Get(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> Create(NoteUserBlank noteUserBlank)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> Update(int id, NoteUserBlank noteUserBlank)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
}