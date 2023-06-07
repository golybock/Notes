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
    private readonly IConfiguration _configuration;
    
    private readonly NoteUserRepository _noteUserRepository;
    private readonly TokensRepository _tokensRepository;
    private readonly AuthManager _authManager;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;

        _authManager = new AuthManager(configuration);
        _noteUserRepository = new NoteUserRepository(configuration);
        _tokensRepository = new TokensRepository(configuration);
    }

    public async Task<IActionResult> Login(LoginBlank loginBlank, HttpContext context)
    {
        var user = await GetUser(loginBlank.Email);

        if (user == null)
            return new UnauthorizedResult();

        if (user.PasswordHash != HashPassword(loginBlank.Password))
            return new UnauthorizedResult();

        await _authManager.SignInAsync(context, user.Email);

        return new OkResult();
    }

    public async Task<IActionResult> Registration(UserBlank userBlank, HttpContext context)
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
        
        await _authManager.SignInAsync(context, userBlank.Email);
        
        #endregion

        return new OkResult();
    }

    public async Task<IActionResult> UpdatePassword(string newPassword, HttpContext context)
    {
        var user = await _authManager.GetCurrentUser(context);
        
        if (user == null)
            return new UnauthorizedResult();

        var res = await _noteUserRepository.UpdatePassword(user.Id, HashPassword(newPassword));

        if (res)
            return new BadRequestObjectResult("Не удалось обновить пароль");
        
        _authManager.SignInAsync(context, user.Email);
 
        return new OkResult();
    }

    #region save in db
    

    
    private async Task<int> CreateUser(UserBlank userBlank)
    {
        string hashedPassword = HashPassword(userBlank.Password);

        var newUser = UserDatabaseBuilder.Create(userBlank, hashedPassword);

        return await _noteUserRepository.Create(newUser);
    }
    
    #endregion

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
        var user = await _noteUserRepository.Get(email);

        return UserDomainBuilder.Create(user);
    }
    
    private async Task<UserDomain?> GetUser(int id)
    {
        var user = await _noteUserRepository.Get(id);

        return UserDomainBuilder.Create(user);
    }
}