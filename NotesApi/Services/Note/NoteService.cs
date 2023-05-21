using Microsoft.AspNetCore.Mvc;
using NotesApi.Repositories.Note;
using NotesApi.Repositories.Note.Tag;
using NotesApi.Services.Interfaces.Note;

namespace NotesApi.Services.Note;

public class NoteService : INoteService
{
    private readonly NoteRepository _noteRepository;
    private readonly TagRepository _tagRepository;
    private readonly NoteTagRepository _noteTagRepository;

    public NoteService(IConfiguration configuration)
    {
        _noteRepository = new NoteRepository(configuration);
        _tagRepository = new TagRepository(configuration);
        _noteTagRepository = new NoteTagRepository(configuration);
    }
    
    public async Task<IActionResult> Get()
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> Get(int id)
    {
        throw new NotImplementedException();
    }
}