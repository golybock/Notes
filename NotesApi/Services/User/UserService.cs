using Blank.User;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Repositories.User;
using NotesApi.Services.Interfaces.User;

namespace NotesApi.Services.User;

public class UserService : IUserService
{
    private readonly UserRepository _userRepository;

    public UserService(IConfiguration configuration)
    {
        _userRepository = new UserRepository(configuration);
    }

    public async Task<IActionResult> Get(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> Get(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> Create(UserBlank userBlank)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> Update(int id, UserBlank userBlank)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> Delete(int id)
    {
        throw new NotImplementedException();
    }
}