using System.ComponentModel.DataAnnotations;
using Blank.User;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Repositories.User;
using NotesApi.Services.Interfaces.User;

namespace NotesApi.Services.User;s

public class UserService : IUserService
{
    private readonly NoteUserRepository _noteUserRepository;

    public UserService(IConfiguration configuration)
    {
        _noteUserRepository = new NoteUserRepository(configuration);
    }
    
    public async Task<IActionResult> Get(string email)
    {
        if (string.IsNullOrEmpty(email))
            return new BadRequestObjectResult("Invalid email");
        
        var user =  await _noteUserRepository.Get(email);

        if (user == null)
            return new NotFoundResult();

        return new OkObjectResult(user);
    }

    public async Task<IActionResult> Update(int id, NoteUserBlank noteUserBlank)
    {
        throw new NotImplementedException();
    }
}