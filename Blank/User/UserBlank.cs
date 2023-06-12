using System.ComponentModel.DataAnnotations;

namespace Blank.User;

public class UserBlank
{
    public string Email { get; set; } = null!;
    
    public string Password { get; set; } = null!;
}