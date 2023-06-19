using Database.User;

namespace Repositories.Repositories.Interfaces.User;

public interface ITokenRepository
{
    public Task<List<TokensDatabase>> GetList(int userId);
    
    public Task<TokensDatabase?> Get(int id);

    public Task<TokensDatabase?> Get(string token, string refreshToken);

    public Task<int> Create(TokensDatabase tokensDatabase);
    
    public Task<bool> Delete(int id);
    
    public Task<bool> Delete(string token, string refreshToken);
}