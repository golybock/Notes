using System.ComponentModel.DataAnnotations;

namespace Blank.User;

public class TokensBlank
{
    [Required]
    public string Token { get; set; } = null!;
    
    [Required]
    public string RefreshToken { get; set; } = null!;
}