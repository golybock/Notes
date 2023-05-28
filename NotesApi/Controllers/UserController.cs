using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blank.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.User;

namespace NotesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(IConfiguration configuration)
    {
        _userService = new UserService(configuration);
    }

    [HttpGet("User"), Authorize]
    public async Task<IActionResult> GetUser()
    {
        return await _userService.Get(User);
    }
    
    [HttpPut("User"), Authorize]
    public async Task<IActionResult> UpdateUser(NoteUserBlank userBlank)
    {
        return await _userService.Update(User, userBlank);
    }
}