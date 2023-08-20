using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using NotesApi.RefreshCookieAuthScheme.AuthManager;
using NotesApi.Services.User.UserManager;
using System.Text;
using Blank.User;

namespace NotesApi.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IAuthManager _authManager;
    private readonly IUserManager _userManager;

    public AuthService(IUserManager userManager, IAuthManager authManager)
    {
        _userManager = userManager;
        _authManager = authManager;
    }

    private async Task<Guid> CreateUser(UserBlank userBlank)
    {
        string hashedPassword = HashPassword(userBlank.Password);

        return await _userManager.Create(userBlank, hashedPassword);
    }

    #region compute hash md5

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
        var user = await _userManager.Get(loginBlank.Email);

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

        var user = await _userManager.Get(userBlank.Email);

        if (user != null)
            return new BadRequestObjectResult("Email already used");

        if (!ValidatePassword(userBlank.Password))
            return new BadRequestObjectResult("Incorrect password");
        
        if(!ValidateEmail(userBlank.Email))
            return new BadRequestObjectResult("Incorrect email");

        #endregion

        #region generate user and tokens

        var id = await CreateUser(userBlank);
        // todo minimaze db requests 
        var newUser = await _userManager.Get(id);

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