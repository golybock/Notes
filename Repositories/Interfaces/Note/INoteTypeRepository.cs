using Database.Note;

namespace Repositories.Interfaces.Note;

public interface INoteTypeRepository
{
    public Task<NoteTypeDatabase?> Get(int id);
    
    public Task<List<NoteTypeDatabase>> Get();
}