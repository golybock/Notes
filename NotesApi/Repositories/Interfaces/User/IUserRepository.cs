using Database.User;

namespace NotesApi.Repositories.Interfaces.User;

public interface IUserRepository
{
    public Task<UserDatabase?> Get(int id);
    
    public Task<UserDatabase?> Get(string email);
}