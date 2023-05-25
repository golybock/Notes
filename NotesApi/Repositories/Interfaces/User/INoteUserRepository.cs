using Database.User;

namespace NotesApi.Repositories.Interfaces.User;

public interface INoteUserRepository
{
    public Task<NoteUserDatabase?> Get(int id);
    
    public Task<NoteUserDatabase?> Get(string email);

    public Task<int> Create(NoteUserDatabase noteUserDatabase);

    public Task<int> Update(int id, NoteUserDatabase noteUserDatabase);
    
    public Task<int> UpdatePassword(int id, string newPassword);

    public Task<int> Delete(int id);
}