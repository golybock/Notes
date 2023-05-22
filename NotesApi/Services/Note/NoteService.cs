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

        for (int i = 0; i < notesView.Count; i++)
            notesView[i].Tags = await GetNoteTags(notesDatabase[i].Id);

        return new OkObjectResult(notesView);
    }

    public async Task<IActionResult> Get(Guid guid)
    {
        var noteDatabase = await _noteRepository.Get(guid);

        if (noteDatabase == null)
            return new NotFoundResult();

        var noteDomain = NoteDomainBuilder.Create(noteDatabase);

        if (noteDatabase.SourcePath != null)
            noteDomain.Text = await GetNoteText(noteDatabase.SourcePath);

        var noteView = NoteViewBuilder.Create(noteDomain);

        noteView.Tags = await GetNoteTags(noteDatabase.Id);

        return new OkObjectResult(noteView);
    }

    public async Task<IActionResult> Create(NoteBlank noteBlank)
    {
        // userId
        var userId = 1;

        var noteDatabase = NoteDatabaseBuilder.Create(noteBlank, userId);

        if (noteBlank.Text != null)
        {
            var path = await WriteNoteText(noteBlank.Text);

            noteDatabase.SourcePath = path;
        }

        noteDatabase.CreationDate = DateTime.Now;
        noteDatabase.EditedDate = DateTime.Now;
        noteDatabase.Guid = Guid.NewGuid();

        var result = await _noteRepository.Create(noteDatabase);
        
        await CreateNoteTags(result, noteBlank.Tags);

        return result > 0 ? new OkObjectResult(noteDatabase.Guid) : new BadRequestResult();
    }

    public async Task<IActionResult> Update(Guid guid, NoteBlank noteBlank)
    {
        var noteDatabase = await _noteRepository.Get(guid);

        if (noteDatabase == null)
            return new NotFoundResult();

        var newNoteDatabase = NoteDatabaseBuilder.Create(noteBlank);

        if (noteBlank.Text != null)
        {
            if (noteDatabase.SourcePath != null)
                await WriteNoteText(noteDatabase.SourcePath, noteBlank.Text);
        }

        newNoteDatabase.EditedDate = DateTime.Now;

        await CreateNoteTags(newNoteDatabase.Id, noteBlank.Tags);
        
        var result = await _noteRepository.Update(guid, newNoteDatabase);

        return result > 0 ? new OkResult() : new BadRequestResult();
    }

    public async Task<IActionResult> Delete(Guid guid)
    {
        var note = await _noteRepository.Get(guid);

        if (note == null)
            return new NotFoundResult();
        
        await _noteTagRepository.DeleteByNote(note.Id);
        
        var result = await _noteRepository.Delete(guid);

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

    /// <summary>
    /// read text from source
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private async Task<string?> GetNoteText(string source)
    {
        string path = "Files/" + source;

        if (!File.Exists(path))
            return null;

        using StreamReader sr = new StreamReader(path);

        return await sr.ReadToEndAsync();
    }

    /// <summary>
    /// Write text into file by source 
    /// </summary>
    /// <param name="fileName">filName </param>
    /// <param name="text">note text</param>
    private async Task WriteNoteText(string fileName, string text)
    {
        string source = $"Files/{fileName}";

        await using StreamWriter sw = new StreamWriter(source);

        await sw.WriteLineAsync(text);
    }

    private async Task CreateNoteTags(int noteId, List<int> noteTags)
    {
        await _noteTagRepository.DeleteByNote(noteId);
        
        foreach (var noteTagId in noteTags)
            await _noteTagRepository.Create(new NoteTagDatabase() {NoteId = noteId, TagId = noteTagId});
    }

    /// <summary>
    /// Generate path file, write into file text and returns filepath
    /// </summary>
    /// <param name="text">note text</param>
    /// <returns>path to file</returns>
    private async Task<string> WriteNoteText(string text)
    {
        string fileName = $"{Guid.NewGuid()}.txt";

        string source = $"Files/{fileName}";

        await using StreamWriter sw = new StreamWriter(source);

        await sw.WriteLineAsync(text);

        return fileName;
    }
}