using Domain.Note;

namespace Domain.User;

public class UserDomain
{
    public Guid Id { get; set; }
    
    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? PasswordHash { get; set; } = null!;
}