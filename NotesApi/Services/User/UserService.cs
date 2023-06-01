using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Blank.User;
using DatabaseBuilder.User;
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
    
    public async Task<IActionResult> Get(ClaimsPrincipal claimsPrincipal)
    {
        var email = claimsPrincipal.Identity?.Name;

        if (email == null)
            return new BadRequestResult();

        if (string.IsNullOrEmpty(email))
            return new BadRequestObjectResult("Invalid email");
        
        var user =  await _noteUserRepository.Get(email);

        if (user == null)
            return new NotFoundResult();

        return new OkObjectResult(user);
    }

    public async Task<IActionResult> Update(ClaimsPrincipal claimsPrincipal, UserBlank userBlank)
    {
        var email = claimsPrincipal.Identity?.Name;

        if (email == null)
            return new BadRequestResult();

        if (string.IsNullOrEmpty(email))
            return new BadRequestObjectResult("Invalid email");

        var userDatabase = UserDatabaseBuilder.Create(userBlank);
        
        var user =  await _noteUserRepository.Update(email, userDatabase);

        if (user <= 0)
            return new BadRequestResult();

        return new OkObjectResult(user);
    }
}