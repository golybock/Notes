using Database.Note.Tag;

namespace NotesApi.Repositories.Interfaces.Note.Tag;

public interface INoteTagRepository
{
    public Task<NoteTagDatabase?> Get(int id);

    public Task<int> Create(NoteTagDatabase noteTagDatabase);
    
    public Task<int> Delete(int id);
    
    public Task<int> DeleteByNote(int noteId);
    
    public Task<int> DeleteByTag(int tagId);
}