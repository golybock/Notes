using Database.Note;

namespace NotesApi.Repositories.Interfaces.Note;

public interface ISharedNotesRepository
{
    public Task<List<SharedNoteDatabase>> Get(Guid noteId);
    
    public Task<List<SharedNoteDatabase>> Get(int userId);

    public Task<int> Create(SharedNoteDatabase sharedNoteDatabase);

    public Task<bool> Update(Guid noteId, int userId, int permissionsLevel);

    public Task<bool> Delete(Guid noteId, int userId);
    
    public Task<bool> DeleteNote(Guid noteId);
}