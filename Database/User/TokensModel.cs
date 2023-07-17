using System.Text.Json.Serialization;

namespace Database.User;

public class TokensModel
{
    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }
    [JsonPropertyName("token")]
    public string Token { get; set; } = null!;
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = null!;
    [JsonPropertyName("ip")]
    public string? Ip { get; set; }
}