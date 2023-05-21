using Database.Note.Tag;
using DomainBuilder.Note;
using DomainBuilder.Note.Tag;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Repositories.Note;
using NotesApi.Repositories.Note.Tag;
using NotesApi.Services.Interfaces.Note;
using ViewBuilder.Note;
using ViewBuilder.Note.Tag;
using Views.Note;
using Views.Note.Tag;

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
        var notesDatabase = await _noteRepository.Get();

        var notesDomain = notesDatabase
            .Select(NoteDomainBuilder.Create)
            .ToList();

        for (int i = 0; i < notesDatabase.Count; i++)
        {
            if (notesDatabase[i].SourcePath != null)
            {
                var sourcePath = notesDatabase[i].SourcePath;
                
                if (sourcePath != null)
                    notesDomain[i].Text = await GetNoteText(sourcePath);
            }
        }
        
        var notesView = notesDomain
            .Select(NoteViewBuilder.Create)
            .ToList();

        foreach (var noteView in notesView)
            noteView.Tags = await GetNoteTags(noteView.Id);

        return new OkObjectResult(notesView);
    }

    public async Task<IActionResult> Get(int id)
    {
        if (id <= 0)
            return new BadRequestObjectResult("Not valid id");
        
        var noteDatabase = await _noteRepository.Get(id);

        if (noteDatabase == null)
            return new NotFoundResult();
        
        var noteDomain = NoteDomainBuilder.Create(noteDatabase);

        if (noteDatabase.SourcePath != null) 
            noteDomain.Text = await GetNoteText(noteDatabase.SourcePath);

        var noteView = NoteViewBuilder.Create(noteDomain);

        noteView.Tags = await GetNoteTags(noteView.Id);
        
        return new OkObjectResult(noteView);
    }

    private async Task<List<TagView>> GetNoteTags(int noteId)
    {
        var tagsDatabase = await _tagRepository.GetNoteTags(noteId);

        var tagsDomain = tagsDatabase
            .Select(TagDomainBuilder.Create)
            .ToList();

        var tagsView = tagsDomain
            .Select(TagViewBuilder.Create)
            .ToList();

        return tagsView;
    }

    private async Task<string?> GetNoteText(string source)
    {
        string path = "Files/" + source;
        
        if (!File.Exists(path)) 
            return null;
        
        using StreamReader sr = new StreamReader(path);
        
        return await sr.ReadToEndAsync();
    }
}