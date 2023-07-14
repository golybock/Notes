namespace Database.User;

public class TokensDatabase
{
    public int Id { get; set; }
    
    public Guid UserId { get; set; }

    public string Token { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public string? Ip { get; set; } = null!;
}