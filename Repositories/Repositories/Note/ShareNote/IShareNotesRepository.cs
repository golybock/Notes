using Database.Note;
using Database.User;

namespace Repositories.Repositories.Note.ShareNote;

public interface IShareNotesRepository
{
    public Task<List<SharedNoteDatabase>> Get(Guid noteId);

    public Task<List<UserDatabase>> GetSharedUsers(Guid noteId);

    public Task<int> Create(SharedNoteDatabase sharedNoteDatabase);

    public Task<bool> Update(Guid noteId, Guid userId, int permissionsLevel);

    public Task<bool> Delete(Guid noteId, Guid userId);
    
    public Task<bool> DeleteNoteShare(Guid noteId);
}