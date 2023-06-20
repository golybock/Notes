using Database.User;

namespace Repositories.Interfaces.User;

public interface IUserRepository
{
    public Task<UserDatabase?> Get(Guid id);
    
    public Task<UserDatabase?> Get(string email);

    public Task<Guid> Create(UserDatabase userDatabase);

    public Task<bool> Update(string email, UserDatabase userDatabase);
    
    public Task<bool> UpdatePassword(Guid id, string newPassword);

    public Task<bool> Delete(Guid id);
}