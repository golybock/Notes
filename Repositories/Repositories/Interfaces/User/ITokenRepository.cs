using Database.User;

namespace Repositories.Repositories.Interfaces.User;

public interface ITokenRepository
{
    public Task<List<TokensDatabase>> GetList(int userId);
    
    public Task<TokensDatabase?> Get(int id);

    public Task<TokensDatabase?> Get(string token, string refreshToken);

    public Task<bool> SetNotActive(int id);

    public Task<int> Create(TokensDatabase tokensDatabase);
    
    public Task<bool> Delete(int id);
}