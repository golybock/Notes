using Database.Note.Tag;

namespace NotesApi.Repositories.Interfaces.Note.Tag;

public interface INoteTagRepository
{
    public Task<NoteTagDatabase?> Get(int id);
}