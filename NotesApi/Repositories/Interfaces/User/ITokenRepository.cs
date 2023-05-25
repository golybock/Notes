using Database.User;

namespace NotesApi.Repositories.Interfaces.User;

public interface ITokenRepository
{
    public Task<List<TokensDatabase>> GetList(int userId);
    
    public Task<TokensDatabase?> Get(int id);

    public Task<TokensDatabase?> Get(string token, string refreshToken);

    public Task<int> SetNotActive(int id);

    public Task<int> Create(TokensDatabase tokensDatabase);
    
    public Task<int> Delete(int id);
}