namespace Database.User;

public class UserDatabase
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; } = null!;
}