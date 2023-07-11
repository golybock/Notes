using Database.Note;

namespace Repositories.Interfaces.Note;

public interface INoteImageRepository
{
    public Task<List<NoteImageDatabase>> GetImages(Guid noteId);
    
    public Task<Guid> Create(NoteImageDatabase noteImageDatabase);

    public Task<bool> Update(Guid id, NoteImageDatabase noteImageDatabase);
    
    public Task<bool> Delete(Guid id);
    
    public Task<bool> Clear(Guid noteId);
}