using Database.User;

namespace Repositories.Repositories.Interfaces.User;

public interface INoteUserRepository
{
    public Task<UserDatabase?> Get(int id);
    
    public Task<UserDatabase?> Get(string email);

    public Task<int> Create(UserDatabase userDatabase);

    public Task<bool> Update(string email, UserDatabase userDatabase);
    
    public Task<bool> UpdatePassword(int id, string newPassword);

    public Task<bool> Delete(int id);
}