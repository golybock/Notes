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
using NotesApi.Auth;
using NotesApi.Services.Interfaces.Note;
using Repositories.Repositories.Note;
using Repositories.Repositories.Note.Tag;
using Repositories.Repositories.User;
using ViewBuilder.Note;
namespace NotesApi.Services.Note;

public class NoteService : INoteService
{
    private readonly NoteRepository _noteRepository;
    private readonly TagRepository _tagRepository;
    private readonly NoteTagRepository _noteTagRepository;
    private readonly ShareNoteRepository _shareNoteRepository;
    private readonly PermissionsLevelRepository _permissionsLevelRepository;
    private readonly NoteTypeRepository _noteTypeRepository;
    private readonly UserRepository _userRepository;
    private readonly AuthManager _authManager;

    public NoteService(IConfiguration configuration)
    {
        _authManager = new AuthManager(configuration);
        _noteRepository = new NoteRepository(configuration);
        _tagRepository = new TagRepository(configuration);
        _noteTagRepository = new NoteTagRepository(configuration);
        _shareNoteRepository = new ShareNoteRepository(configuration);
        _permissionsLevelRepository = new PermissionsLevelRepository(configuration);
        _noteTypeRepository = new NoteTypeRepository(configuration);
        _userRepository = new UserRepository(configuration);
    }

    #region controller funcs (use in controllers)
    
    public async Task<IActionResult> Get(HttpContext context, string email)
    {
        var user = await GetUser(email);

        if (user == null)
            return new UnauthorizedResult();

        var notesDomain = await GetNotes(user.Id);

        var notesView = notesDomain
            .Select(NoteViewBuilder.Create)
            .ToList();

        return new OkObjectResult(notesView);
    }

    public async Task<IActionResult> GetShared(HttpContext context)
    {
        var signed = await _authManager.IsSigned(context);

        if (signed == null)
            return new UnauthorizedResult();

        var notesDomain = await GetSharedNotes(signed.Id);

        var notesView = notesDomain
            .Select(NoteViewBuilder.Create)
            .ToList();

        return new OkObjectResult(notesView);
    }

    public async Task<IActionResult> Get(HttpContext context, Guid id)
    {
        var signed = await _authManager.IsSigned(context);

        if (signed == null)
            return new UnauthorizedResult();
        var noteDomain = await GetNote(id);

        if (noteDomain == null)
            return new NotFoundResult();

        if (noteDomain.User?.Email != signed.Email)
            return new BadRequestObjectResult("Access denied");

        var noteView = NoteViewBuilder.Create(noteDomain);

        return new OkObjectResult(noteView);
    }

    public async Task<IActionResult> Create(HttpContext context, NoteBlank noteBlank)
    {
        var signed = await _authManager.IsSigned(context);

        if (signed == null)
            return new UnauthorizedResult();

        var noteDatabase = NoteDatabaseBuilder.Create(noteBlank, signed.Id);

        if (noteBlank.Text != null)
        {
            var path = await NoteFileManager.CreateNoteText(noteBlank.Text);

            noteDatabase.SourcePath = path;
        }

        noteDatabase.CreationDate = DateTime.Now;
        noteDatabase.EditedDate = DateTime.Now;
        noteDatabase.Id = Guid.NewGuid();

        var result = await _noteRepository.Create(noteDatabase);

        await CreateNoteTags(result, noteBlank.Tags);

        return result != Guid.Empty ? new OkObjectResult(noteDatabase.Id) : new BadRequestResult();
    }

    public async Task<IActionResult> Update(HttpContext context, Guid guid, NoteBlank noteBlank)
    {
        var noteDatabase = await _noteRepository.Get(guid);

        if (noteDatabase == null)
            return new NotFoundResult();

        var newNoteDatabase = NoteDatabaseBuilder.Create(noteBlank);

        if (noteBlank.Text != null)
        {
            if (noteDatabase.SourcePath != null)
                await NoteFileManager.UpdateNoteText(noteDatabase.SourcePath, noteBlank.Text);
        }

        newNoteDatabase.EditedDate = DateTime.Now;

        await CreateNoteTags(newNoteDatabase.Id, noteBlank.Tags);

        var result = await _noteRepository.Update(guid, newNoteDatabase);

        return result ? new OkResult() : new BadRequestResult();
    }

    public async Task<IActionResult> Share(HttpContext context, ShareBlank shareBlank)
    {
        var signed = await _authManager.IsSigned(context);

        if (signed == null)
            return new UnauthorizedResult();

        var note = await GetNote(shareBlank.Id);

        if (note == null)
            return new NotFoundResult();

        var sharedUser = await _userRepository.Get(shareBlank.Email);

        if (sharedUser == null)
            return new NotFoundResult();

        await _noteRepository.UpdateType(note.Id, 2);

        var shareNote = new SharedNoteDatabase()
        {
            NoteId = shareBlank.Id,
            PermissionsLevelId = shareBlank.PermissionLevel,
            UserId = sharedUser.Id
        };

        var res = await _shareNoteRepository.Create(shareNote);

        return res > 0 ? new OkResult() : new BadRequestObjectResult("Invalid data");
    }

    public async Task<IActionResult> UpdateShare(HttpContext context, ShareBlank shareBlank)
    {
        var signed = await _authManager.IsSigned(context);

        if (signed == null)
            return new UnauthorizedResult();

        var note = await GetNote(shareBlank.Id);

        if (note == null)
            return new NotFoundResult();

        var sharedUser = await _userRepository.Get(shareBlank.Email);

        if (sharedUser == null)
            return new NotFoundResult();

        var res = await _shareNoteRepository.Update(shareBlank.Id, sharedUser.Id, shareBlank.PermissionLevel);

        return res ? new OkResult() : new BadRequestObjectResult("Invalid data");
    }

    public async Task<IActionResult> DeleteShare(HttpContext context, Guid id, string email)
    {
        var signed = await _authManager.IsSigned(context);

        if (signed == null)
            return new UnauthorizedResult();

        var note = await GetNote(id);

        if (note == null)
            return new NotFoundResult();

        var sharedUser = await _userRepository.Get(email);

        if (sharedUser == null)
            return new NotFoundResult();

        var res = await _shareNoteRepository.Delete(id, sharedUser.Id);

        return res ? new OkResult() : new BadRequestObjectResult("Error delete");
    }

    public async Task<IActionResult> Delete(HttpContext context, Guid id)
    {
        var signed = await _authManager.IsSigned(context);

        if (signed == null)
            return new UnauthorizedResult();
        var note = await _noteRepository.Get(id);

        if (note == null)
            return new NotFoundResult();

        if (note.OwnerId != signed.Id)
            return new BadRequestObjectResult("Access denied");

        await _noteTagRepository.DeleteByNote(note.Id);
        await _shareNoteRepository.DeleteNote(note.Id);

        var result = await _noteRepository.Delete(id);

        return result ? new OkResult() : new BadRequestObjectResult("Error delete");
    }

    #endregion

    #region get funcs

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
                    t.Text = await NoteFileManager.GetNoteText(sourcePath);
            }
        }

        return notesDomain;
    }

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
                    t.Text = await NoteFileManager.GetNoteText(sourcePath);
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
            noteDomain.Text = await NoteFileManager.GetNoteText(noteDatabase.SourcePath);

        var type = await _noteTypeRepository.Get(noteDomain.TypeId);

        var user = await _userRepository.Get(noteDomain.OwnerId);

        noteDomain.User = UserDomainBuilder.Create(user);

        noteDomain.Type = NoteTypeDomainBuilder.Create(type);

        noteDomain.Tags = await GetNoteTags(noteDatabase.Id);

        return noteDomain;
    }

    private async Task<UserDomain?> GetUser(string email)
    {
        var user = await _userRepository.Get(email);

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
}