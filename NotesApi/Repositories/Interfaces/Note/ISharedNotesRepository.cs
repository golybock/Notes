using Database.Note;

namespace NotesApi.Repositories.Interfaces.Note;

public interface ISharedNotesRepository
{
    public Task<SharedNoteDatabase> Get(Guid noteId);
    public Task<List<SharedNoteDatabase>> Get(int userId);
}