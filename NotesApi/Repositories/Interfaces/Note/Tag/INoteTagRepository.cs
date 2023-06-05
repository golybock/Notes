using Database.Note.Tag;

namespace NotesApi.Repositories.Interfaces.Note.Tag;

public interface INoteTagRepository
{
    public Task<NoteTagDatabase?> Get(int id);

    public Task<int> Create(NoteTagDatabase noteTagDatabase);
    
    public Task<bool> Delete(int id);
    
    public Task<bool> DeleteByNote(Guid noteId);
    
    public Task<bool> DeleteByTag(int tagId);
}