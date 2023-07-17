using Database.Note;

namespace Repositories.Repositories.Note.NoteType;

public interface INoteTypeRepository
{
    public Task<NoteTypeDatabase?> Get(int id);
    
    public Task<List<NoteTypeDatabase>> Get();
}