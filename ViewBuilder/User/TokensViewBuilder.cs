using Domain.User;
using Views.User;

namespace ViewBuilder.User;

public class TokensViewBuilder
{
    public static TokensView Create(TokensDomain tokensDomain)
    {
        return new TokensView()
        {
            Token = tokensDomain.Token,
            RefreshToken = tokensDomain.RefreshToken
        };
    }
    
    public static TokensView Create(string token, string refreshToken)
    {
        return new TokensView()
        {
            Token = token,
            RefreshToken = refreshToken
        };
    }
}