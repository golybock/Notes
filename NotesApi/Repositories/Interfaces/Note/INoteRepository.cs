using Database.Note;

namespace NotesApi.Repositories.Interfaces.Note;

public interface INoteRepository
{
    public Task<NoteDatabase?> Get(int id);
    
    public Task<List<NoteDatabase>> Get();

    public Task<int> Create(NoteDatabase noteDatabase);

    public Task<int> Update(int id, NoteDatabase noteDatabase);

    public Task<int> Delete(int id);
}