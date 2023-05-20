using Database.Note;

namespace NotesApi.Repositories.Interfaces.Note;

public interface INoteRepository
{
    public Task<NoteDatabase> Get(int id);
    
    public Task<List<NoteDatabase>> Get();
}