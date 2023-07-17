using Database.Note.Tag;

namespace Repositories.Repositories.Note.Tag;

public interface ITagRepository
{
    // tag
    public Task<TagDatabase?> Get(Guid id);
    
    // all tags
    public Task<List<TagDatabase>> Get();

    // note tags
    public Task<List<TagDatabase>> GetNoteTags(Guid noteId);

    // create tag
    public Task<int> Create(TagDatabase tagDatabase);
    
    // create note-tag
    public Task<int> Create(NoteTagDatabase noteTagDatabase);
    
    public Task<bool> DeleteNoteTags(Guid noteId);

    public Task<bool> Delete(int id);
}