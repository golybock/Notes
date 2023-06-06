using Database.Note.Tag;

namespace Repositories.Repositories.Interfaces.Note.Tag;

public interface ITagRepository
{
    public Task<TagDatabase?> Get(int id);
    
    public Task<List<TagDatabase>> Get();

    public Task<List<TagDatabase>> GetNoteTags(Guid noteId);

    public Task<int> Create(TagDatabase tagDatabase);
    
    public Task<bool> Update(int id, TagDatabase tagDatabase);

    public Task<bool> Delete(int id);
}