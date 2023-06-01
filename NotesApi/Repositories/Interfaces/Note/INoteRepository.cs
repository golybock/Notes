using Database.Note;

namespace NotesApi.Repositories.Interfaces.Note;

public interface INoteRepository
{
    public Task<NoteDatabase?> Get(Guid id);
    
    public Task<List<NoteDatabase>> Get();

    public Task<Guid> Create(NoteDatabase noteDatabase);

    public Task<Guid> Update(Guid id, NoteDatabase noteDatabase);

    public Task<Guid> Delete(Guid id);
}