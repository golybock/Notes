using System.ComponentModel.DataAnnotations;

namespace Blank.User;

public class LoginBlank
{
    [Required, EmailAddress]
    public string Email { get; set; } = null!;
    [Required, MinLength(8)]
    public string Password { get; set; } = null!;
}