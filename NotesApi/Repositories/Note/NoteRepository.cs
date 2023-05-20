using Database.Note;
using NotesApi.Repositories.Interfaces.Note;

namespace NotesApi.Repositories.Note;

public class NoteRepository : RepositoryBase, INoteRepository
{
    public NoteRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<NoteDatabase> Get(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<NoteDatabase>> Get()
    {
        throw new NotImplementedException();
    }
}