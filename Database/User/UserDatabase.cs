namespace Database.User;

public class UserDatabase
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; } = null!;

    public string Name { get; set; } = null!;
}