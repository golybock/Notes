using Database.User;
using Domain.User;

namespace DomainBuilder.User;

public class TokensDomainBuilder
{
    public static TokensDomain Create(TokensDatabase tokensDatabase)
    {
        return new TokensDomain()
        {
            Token = tokensDatabase.Token,
            RefreshToken = tokensDatabase.RefreshToken
        };
    }
    
    public static TokensDomain Create(string token, string refreshToken)
    {
        return new TokensDomain()
        {
            Token = token,
            RefreshToken = refreshToken
        };
    }
}