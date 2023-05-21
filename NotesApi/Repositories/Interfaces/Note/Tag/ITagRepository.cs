using Database.Note.Tag;

namespace NotesApi.Repositories.Interfaces.Note.Tag;

public interface ITagRepository
{
    public Task<TagDatabase?> Get(int id);
    
    public Task<List<TagDatabase>> Get();

    public Task<List<TagDatabase>> GetNoteTags(int noteId);
}