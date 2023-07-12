using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Blank.User;
using DatabaseBuilder.User;
using Domain.User;
using DomainBuilder.User;
using Microsoft.AspNetCore.Mvc;
using NotesApi.RefreshCookieAuthScheme;
using NotesApi.RefreshCookieAuthScheme.AuthManager;
using NotesApi.Services.Interfaces.User;
using NotesApi.Services.User;
using Repositories.Repositories.User;

namespace NotesApi.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserRepository _userRepository;
    private readonly IAuthManager _authManager;
    private readonly UserManager _userManager;

    public AuthService(IConfiguration configuration, IAuthManager authManager)
    {
        _authManager = authManager;
        _userManager = new UserManager(configuration);
        _userRepository = new UserRepository(configuration);
    }

    private async Task<Guid> CreateUser(UserBlank userBlank)
    {
        string hashedPassword = HashPassword(userBlank.Password);

        var id = Guid.NewGuid();

        var newUser = UserDatabaseBuilder.Create(userBlank, hashedPassword, id);

        return await _userRepository.Create(newUser);
    }

    #region compute hash

    // md5 hash password
    private string HashPassword(string password)
    {
        byte[] input = Encoding.UTF8.GetBytes(password);

        using var md5 = MD5.Create();
        var output = md5.ComputeHash(input);

        return Convert.ToBase64String(output);
    }
    
    // todo refactor in db to bytes
    private byte[] HashPasswordBytes(string password)
    {
        byte[] input = Encoding.UTF8.GetBytes(password);

        using var md5 = MD5.Create();
        return md5.ComputeHash(input);
    }

    #endregion

    public async Task<IActionResult> SignIn(HttpContext context, LoginBlank loginBlank)
    {
        var user = await _userManager.GetUser(loginBlank.Email);

        if (user == null)
            return new UnauthorizedResult();

        if (user.PasswordHash != HashPassword(loginBlank.Password))
            return new UnauthorizedResult();

        await _authManager.SignInAsync(context, user);

        return new OkResult();
    }

    public async Task<IActionResult> SignUp(HttpContext context, UserBlank userBlank)
    {
        #region check client data

        var user = await _userManager.GetUser(userBlank.Email);

        if (user != null)
            return new BadRequestObjectResult("Такой email уже зарегистрирован");

        if (!ValidatePassword(userBlank.Password))
            return new BadRequestObjectResult("Неверный формат пароля");
        
        if(!ValidateEmail(userBlank.Email))
            return new BadRequestObjectResult("Неверный формат почты");

        #endregion

        #region generate user and tokens

        var id = await CreateUser(userBlank);
        // todo minimaze db requests 
        var newUser = await _userManager.GetUser(id);

        if (newUser == null)
            return new BadRequestResult();
        
        await _authManager.SignInAsync(context, newUser);
        
        #endregion

        return new OkResult();
    }

    public async Task<IActionResult> SignOut(HttpContext context)
    {
        await _authManager.SignOutAsync(context);

        return new OkResult();
    }
    
    #region validation

    // validate password 
    private bool ValidatePassword(string password) =>
        password.Any(char.IsLetter) &&
        password.Any(char.IsDigit) &&
        password.Any(char.IsUpper) &&
        password.Any(char.IsLower) &&
        password.Length >= 8;

    // validate email
    private bool ValidateEmail(string email) =>
        new EmailAddressAttribute().IsValid(email);

    #endregion
}