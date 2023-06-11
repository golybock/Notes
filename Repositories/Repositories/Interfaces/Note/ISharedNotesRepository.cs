using Database.Note;
using Database.User;

namespace Repositories.Repositories.Interfaces.Note;

public interface ISharedNotesRepository
{
    public Task<List<SharedNoteDatabase>> Get(Guid noteId);
    
    public Task<List<SharedNoteDatabase>> Get(int userId);

    public Task<List<UserDatabase>> GetSharedUsers(Guid noteId);

    public Task<int> Create(SharedNoteDatabase sharedNoteDatabase);

    public Task<bool> Update(Guid noteId, int userId, int permissionsLevel);

    public Task<bool> Delete(Guid noteId, int userId);
    
    public Task<bool> DeleteNote(Guid noteId);
}