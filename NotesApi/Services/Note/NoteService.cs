using System.Security.Claims;
using Blank.Note;
using Database.Note;
using Database.Note.Tag;
using DatabaseBuilder.Note;
using Domain.Note;
using Domain.Note.Tag;
using Domain.User;
using DomainBuilder.Note;
using DomainBuilder.Note.Tag;
using DomainBuilder.User;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Repositories.Note;
using NotesApi.Repositories.Note.Tag;
using NotesApi.Repositories.User;
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
    private readonly ShareNoteRepository _shareNoteRepository;
    private readonly PermissionsLevelRepository _permissionsLevelRepository;
    private readonly NoteTypeRepository _noteTypeRepository;
    private readonly NoteUserRepository _noteUserRepository;

    public NoteService(IConfiguration configuration)
    {
        _noteRepository = new NoteRepository(configuration);
        _tagRepository = new TagRepository(configuration);
        _noteTagRepository = new NoteTagRepository(configuration);
        _shareNoteRepository = new ShareNoteRepository(configuration);
        _permissionsLevelRepository = new PermissionsLevelRepository(configuration);
        _noteTypeRepository = new NoteTypeRepository(configuration);
        _noteUserRepository = new NoteUserRepository(configuration);
    }

    #region controller funcs (use in controllers)

    /// <summary>
    /// Get user notes
    /// </summary>
    /// <param name="claims">user identity</param>
    /// <returns>user notes</returns>
    public async Task<IActionResult> Get(ClaimsPrincipal claims)
    {
        var user = await GetUser(claims);

        if (user == null)
            return new UnauthorizedResult();

        var notesDomain = await GetNotes(user.Id);

        var notesView = notesDomain.Select(NoteViewBuilder.Create).ToList();

        return new OkObjectResult(notesView);
    }
    
    /// <summary>
    /// Get user notes
    /// </summary>
    /// <param name="claims">user identity</param>
    /// <returns>user notes</returns>
    public async Task<IActionResult> GetShared(ClaimsPrincipal claims)
    {
        var user = await GetUser(claims);

        if (user == null)
            return new UnauthorizedResult();

        var notesDomain = await GetSharedNotes(user.Id);
        
        var notesView = notesDomain.Select(NoteViewBuilder.Create).ToList();

        return new OkObjectResult(notesView);
    }

    /// <summary>
    /// Get full note
    /// </summary>
    /// <param name="claims">user identity</param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Get(ClaimsPrincipal claims, Guid id)
    {
        var user = await GetUser(claims);

        if (user == null)
            return new UnauthorizedResult();

        var noteDomain = await GetNote(id);

        if (noteDomain?.User?.Email != user.Email)
            return new BadRequestObjectResult("Access denied");

        if (noteDomain == null)
            return new NotFoundResult();

        var noteView = NoteViewBuilder.Create(noteDomain);

        return new OkObjectResult(noteView);
    }
    
    public async Task<IActionResult> Create(ClaimsPrincipal claims, NoteBlank noteBlank)
    {
        var user = await GetUser(claims);

        if (user == null)
            return new UnauthorizedResult();

        var noteDatabase = NoteDatabaseBuilder.Create(noteBlank, user.Id);

        if (noteBlank.Text != null)
        {
            var path = await WriteNoteText(noteBlank.Text);

            noteDatabase.SourcePath = path;
        }

        noteDatabase.CreationDate = DateTime.Now;
        noteDatabase.EditedDate = DateTime.Now;
        noteDatabase.Id = Guid.NewGuid();

        var result = await _noteRepository.Create(noteDatabase);

        await CreateNoteTags(result, noteBlank.Tags);

        return result != Guid.Empty ? new OkObjectResult(noteDatabase.Id) : new BadRequestResult();
    }

    public async Task<IActionResult> Update(ClaimsPrincipal principal, Guid guid, NoteBlank noteBlank)
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

    public async Task<IActionResult> Share(ClaimsPrincipal claims, ShareBlank shareBlank)
    {
        var user = await GetUser(claims);

        if (user == null)
            return new UnauthorizedResult();

        var note = await GetNote(shareBlank.Id);

        if (note == null)
            return new NotFoundResult();

        var sharedUser = await _noteUserRepository.Get(shareBlank.Email);

        if (sharedUser == null)
            return new NotFoundResult();

        await _noteRepository.UpdateType(note.Id, 2);
        
        var res = await _shareNoteRepository.Create(new SharedNoteDatabase()
            {NoteId = shareBlank.Id, PermissionsLevelId = shareBlank.PermissionLevel, UserId = sharedUser.Id});

        return res > 0 ? new OkResult() : new BadRequestObjectResult("Invalid data");
    }

    public async Task<IActionResult> UpdateShare(ClaimsPrincipal claims, ShareBlank shareBlank)
    {
        var user = await GetUser(claims);

        if (user == null)
            return new UnauthorizedResult();

        var note = await GetNote(shareBlank.Id);

        if (note == null)
            return new NotFoundResult();

        var sharedUser = await _noteUserRepository.Get(shareBlank.Email);

        if (sharedUser == null)
            return new NotFoundResult();

        var res = await _shareNoteRepository.Update(shareBlank.Id, sharedUser.Id, shareBlank.PermissionLevel);

        return res > 0 ? new OkResult() : new BadRequestObjectResult("Invalid data");
    }

    public async Task<IActionResult> DeleteShare(ClaimsPrincipal claims, Guid id, string email)
    {
        var user = await GetUser(claims);

        if (user == null)
            return new UnauthorizedResult();

        var note = await GetNote(id);

        if (note == null)
            return new NotFoundResult();

        var sharedUser = await _noteUserRepository.Get(email);

        if (sharedUser == null)
            return new NotFoundResult();

        var res = await _shareNoteRepository.Delete(id, sharedUser.Id);

        return res > 0 ? new OkResult() : new BadRequestObjectResult("Error delete");
    }

    public async Task<IActionResult> Delete(ClaimsPrincipal claims, Guid id)
    {
        var user = await GetUser(claims);

        if (user == null)
            return new UnauthorizedResult();

        var note = await _noteRepository.Get(id);

        if (note == null)
            return new NotFoundResult();

        if (note.OwnerId != user.Id)
            return new BadRequestObjectResult("Access denied");

        await _noteTagRepository.DeleteByNote(note.Id);
        await _shareNoteRepository.DeleteNote(note.Id);

        var result = await _noteRepository.Delete(id);

        return result > 0 ? new OkResult() : new BadRequestObjectResult("Error delete");
    }

    #endregion

    #region get funcs

    /// <summary>
    /// Get user notes
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>list of user notes</returns>
    private async Task<List<NoteDomain>> GetNotes(int userId)
    {
        var notesDatabase = await _noteRepository.Get(userId);

        var notesDomain = notesDatabase
            .Select(NoteDomainBuilder.Create)
            .ToList();

        foreach (var t in notesDomain)
        {
            var type = await _noteTypeRepository.Get(t.TypeId);

            t.Type = NoteTypeDomainBuilder.Create(type);

            t.Tags = await GetNoteTags(t.Id);

            if (t.SourcePath != null)
            {
                var sourcePath = t.SourcePath;

                if (sourcePath != null)
                    t.Text = await GetNoteText(sourcePath);
            }
        }

        return notesDomain;
    }

    /// <summary>
    /// Get user notes
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>list of shared user notes</returns>
    private async Task<List<NoteDomain>> GetSharedNotes(int userId)
    {
        var notesDatabase = await _noteRepository.GetShared(userId);

        var notesDomain = notesDatabase
            .Select(NoteDomainBuilder.Create)
            .ToList();

        foreach (var t in notesDomain)
        {
            var type = await _noteTypeRepository.Get(t.TypeId);

            t.Tags = await GetNoteTags(t.Id);

            t.Type = NoteTypeDomainBuilder.Create(type);

            if (t.SourcePath != null)
            {
                var sourcePath = t.SourcePath;

                if (sourcePath != null)
                    t.Text = await GetNoteText(sourcePath);
            }
        }

        return notesDomain;
    }

    private async Task<NoteDomain?> GetNote(Guid id)
    {
        var noteDatabase = await _noteRepository.Get(id);

        if (noteDatabase == null)
            return null;

        var noteDomain = NoteDomainBuilder.Create(noteDatabase);

        if (noteDatabase.SourcePath != null)
            noteDomain.Text = await GetNoteText(noteDatabase.SourcePath);

        var type = await _noteTypeRepository.Get(noteDomain.TypeId);

        var user = await _noteUserRepository.Get(noteDomain.OwnerId);

        noteDomain.User = UserDomainBuilder.Create(user);

        noteDomain.Type = NoteTypeDomainBuilder.Create(type);

        noteDomain.Tags = await GetNoteTags(noteDatabase.Id);

        return noteDomain;
    }

    private async Task<UserDomain?> GetUser(ClaimsPrincipal claims)
    {
        var user = await _noteUserRepository.Get(claims.Identity?.Name!);

        return UserDomainBuilder.Create(user);
    }

    #endregion

    #region note tags

    private async Task<List<TagDomain>> GetNoteTags(Guid noteId)
    {
        var tagsDatabase = await _tagRepository.GetNoteTags(noteId);

        var tagsDomain = tagsDatabase
            .Select(TagDomainBuilder.Create)
            .ToList();

        return tagsDomain;
    }

    private async Task CreateNoteTags(Guid noteId, List<int> noteTags)
    {
        await _noteTagRepository.DeleteByNote(noteId);

        foreach (var noteTagId in noteTags)
            await _noteTagRepository.Create(new NoteTagDatabase() {NoteId = noteId, TagId = noteTagId});
    }

    #endregion

    #region note text

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

    #endregion
}