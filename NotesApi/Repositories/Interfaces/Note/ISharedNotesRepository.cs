using Database.Note;

namespace NotesApi.Repositories.Interfaces.Note;

public interface ISharedNotesRepository
{
    public Task<List<SharedNoteDatabase>> Get(Guid noteId);
    
    public Task<List<SharedNoteDatabase>> Get(int userId);

    public Task<int> Create(SharedNoteDatabase sharedNoteDatabase);

    public Task<int> Update(int id, SharedNoteDatabase sharedNoteDatabase);

    public Task<int> Delete(int id);
    
    public Task<int> Delete(Guid noteId);
}