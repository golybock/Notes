using Database.Note;

namespace Repositories.Repositories.Interfaces.Note;

public interface INoteTypeRepository
{
    public Task<NoteTypeDatabase?> Get(int id);
    
    public Task<List<NoteTypeDatabase>> Get();
}