using Database.Note;

namespace Repositories.Repositories.Interfaces.Note;

public interface INoteRepository
{
    public Task<NoteDatabase?> Get(Guid id);
    
    public Task<List<NoteDatabase>> Get();
    
    public Task<List<NoteDatabase>> Get(int userId);
    
    public Task<List<NoteDatabase>> GetShared(int userId);

    public Task<Guid> Create(NoteDatabase noteDatabase);

    public Task<bool> Update(Guid id, NoteDatabase noteDatabase);
    
    public Task<bool> UpdateType(Guid id, int type);

    public Task<bool> Delete(Guid id);
}