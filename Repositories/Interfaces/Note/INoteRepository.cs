using Database.Note;

namespace Repositories.Interfaces.Note;

public interface INoteRepository
{
    public Task<NoteDatabase?> GetNote(Guid id);

    public Task<List<NoteDatabase>> GetNotes(Guid userId);
    
    public Task<List<NoteDatabase>> GetShared(Guid userId);

    public Task<Guid> Create(NoteDatabase noteDatabase);

    public Task<bool> Update(Guid id, NoteDatabase noteDatabase);
    
    public Task<bool> UpdateType(Guid id, int type);

    public Task<NoteDatabase?> GetSharedNote(Guid userId, Guid noteId);

    public Task<bool> Delete(Guid id);
}