using Blank.Note;
using Database.Note.Tag;
using DatabaseBuilder.Note;
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

    public async Task<IActionResult> Create(NoteBlank noteBlank)
    {
        // userId
        var userId = 3;
        
        var noteDatabase = NoteDatabaseBuilder.Create(noteBlank, userId);

        if (noteBlank.Text != null)
        {
            var path = await WriteNoteText(noteBlank.Text);

            noteDatabase.SourcePath = path;
        }

        noteDatabase.LastEditDate = DateTime.Now;

        var result = await _noteRepository.Create(noteDatabase);

        return result > 0 ? new OkObjectResult(result) : new BadRequestResult();
    }

    public async Task<IActionResult> Update(int id, NoteBlank blank)
    {
        var noteDatabase = await _noteRepository.Get(id);

        if (noteDatabase == null)
            return new NotFoundResult();
        
        var newNoteDatabase = NoteDatabaseBuilder.Create(blank);

        if (blank.Text != null)
        {
            if (noteDatabase.SourcePath != null)
                await WriteNoteText(noteDatabase.SourcePath, blank.Text);
        }

        noteDatabase.LastEditDate = DateTime.Now;

        var result = await _noteRepository.Update(id, newNoteDatabase);

        return result > 0 ? new OkResult() : new BadRequestResult();
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _noteRepository.Delete(id);
        
        return result > 0 ? new OkResult() : new BadRequestResult();
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

    private async Task WriteNoteText(string source, string text)
    {
        await using StreamWriter sw = new StreamWriter(source);
        
        await sw.WriteLineAsync(text);
    }
    
    private async Task<string> WriteNoteText(string text)
    {
        string source = Guid.NewGuid().ToString();
        
        await using StreamWriter sw = new StreamWriter(source);
        
        await sw.WriteLineAsync(text);

        return source;
    }
}