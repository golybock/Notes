using Database.Note;

namespace NotesApi.Repositories.Interfaces.Note;

public interface ISharedNotesRepository
{
    public Task<List<SharedNoteDatabase>> Get(Guid noteId);
    
    public Task<List<SharedNoteDatabase>> Get(int userId);

    public Task<int> Create(SharedNoteDatabase sharedNoteDatabase);

    public Task<int> Update(Guid noteId, int userId, int permissionsLevel);

    public Task<int> Delete(Guid noteId, int userId);
    
    public Task<int> DeleteNote(Guid noteId);
}