using Database.User;

namespace NotesApi.Repositories.Interfaces.User;

public interface INoteUserRepository
{
    public Task<UserDatabase?> Get(int id);
    
    public Task<UserDatabase?> Get(string email);

    public Task<int> Create(UserDatabase userDatabase);

    public Task<int> Update(int id, UserDatabase userDatabase);
    
    public Task<int> UpdatePassword(int id, string newPassword);

    public Task<int> Delete(int id);
}