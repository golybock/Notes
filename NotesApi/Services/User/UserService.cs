using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Blank.User;
using DatabaseBuilder.User;
using Domain.User;
using DomainBuilder.User;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Auth;
using NotesApi.Services.Interfaces.User;
using Repositories.Repositories.User;
using ViewBuilder.User;

namespace NotesApi.Services.User;

public class UserService : IUserService
{
    private readonly UserRepository _userRepository;
    private readonly AuthManager _authManager;
    
    public UserService(IConfiguration configuration)
    {
        _authManager = new AuthManager(configuration);
        _userRepository = new UserRepository(configuration);
    }

    public async Task<IActionResult> Get(HttpContext context)
    {
        var signed = await _authManager.IsSigned(context);

        if (signed == null)
            return new UnauthorizedResult();

        var userView = UserViewBuilder.Create(signed);
        
        return new OkObjectResult(userView);
    }

    public async Task<IActionResult> Update(HttpContext context, UserBlank userBlank)
    {
        var signed = await _authManager.IsSigned(context);

        if (signed == null)
            return new UnauthorizedResult();
        
        var userDatabase = UserDatabaseBuilder.Create(userBlank);

        var updated = await _userRepository.Update(signed.Email, userDatabase);

        if (!updated)
            return new BadRequestResult();

        return new OkResult();
    }

}