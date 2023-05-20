using Database.Note.Tag;
using NotesApi.Repositories.Interfaces;
using NotesApi.Repositories.Interfaces.Note.Tag;

namespace NotesApi.Repositories.Note.Tag;

public class TagRepository : RepositoryBase, ITagRepository
{
    public TagRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<TagDatabase> Get(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TagDatabase>> Get()
    {
        throw new NotImplementedException();
    }
}