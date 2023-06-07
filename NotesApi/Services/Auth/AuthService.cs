using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using Blank.User;
using DatabaseBuilder.User;
using Domain.User;
using DomainBuilder.User;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Auth;
using NotesApi.Services.Interfaces.User;
using Repositories.Repositories.User;

namespace NotesApi.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserRepository _userRepository;
    private readonly AuthManager _authManager;

    public AuthService(IConfiguration configuration, HttpContext context)
    {
        _authManager = new AuthManager(configuration, context);
        _userRepository = new UserRepository(configuration);
    }

    private async Task<int> CreateUser(UserBlank userBlank)
    {
        string hashedPassword = HashPassword(userBlank.Password);

        var newUser = UserDatabaseBuilder.Create(userBlank, hashedPassword);

        return await _userRepository.Create(newUser);
    }

    #region generating data

    // md5 hash password
    private string HashPassword(string password)
    {
        byte[] input = Encoding.UTF8.GetBytes(password);

        using var md5 = MD5.Create();
        var output = md5.ComputeHash(input);

        return Convert.ToBase64String(output);
    }

    #endregion

    
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

    private async Task<UserDomain?> GetUser(string email)
    {
        var user = await _userRepository.Get(email);

        return UserDomainBuilder.Create(user);
    }
    
    private async Task<UserDomain?> GetUser(int id)
    {
        var user = await _userRepository.Get(id);

        return UserDomainBuilder.Create(user);
    }

    public async Task<IActionResult> SignIn(LoginBlank loginBlank)
    {
        var user = await GetUser(loginBlank.Email);

        if (user == null)
            return new UnauthorizedResult();

        if (user.PasswordHash != HashPassword(loginBlank.Password))
            return new UnauthorizedResult();

        await _authManager.SignInAsync(user);

        return new OkResult();
    }

    public async Task<IActionResult> SignUp(UserBlank userBlank)
    {
        #region check client data

        var user = await GetUser(userBlank.Email);

        if (user != null)
            return new BadRequestObjectResult("Такой email уже зарегистрирован");

        if (!ValidatePassword(userBlank.Password))
            return new BadRequestObjectResult("Неверный формат пароля");
        
        if(!ValidateEmail(userBlank.Email))
            return new BadRequestObjectResult("Неверный формат почты");

        #endregion

        #region generate user and tokens

        var id = await CreateUser(userBlank);

        var newUser = await GetUser(id);
        
        await _authManager.SignInAsync(newUser);
        
        #endregion

        return new OkResult();
    }

    public Task<IActionResult> SignOut()
    {
        _authManager.SignOut();

        return Task.FromResult<IActionResult>(new OkResult());
    }
}