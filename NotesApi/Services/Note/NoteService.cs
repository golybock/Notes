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
using NotesApi.Enums;
using NotesApi.RefreshCookieAuthScheme.AuthManager;
using NotesApi.Services.Interfaces.Note;
using NotesApi.Services.User;
using Repositories.Repositories.Note;
using Repositories.Repositories.Note.Tag;
using Repositories.Repositories.User;
using ViewBuilder.Note;

namespace NotesApi.Services.Note;

// todo optimaze
public class NoteService : INoteService
{
    private readonly NoteRepository _noteRepository;
    private readonly TagRepository _tagRepository;
    private readonly ShareNoteRepository _shareNoteRepository;
    private readonly NoteTypeRepository _noteTypeRepository;
    private readonly UserRepository _userRepository;
    private readonly AuthManager _authManager;
    private readonly UserManager _userManager;

    public NoteService(IConfiguration configuration)
    {
        _noteRepository = new NoteRepository(configuration);
        _tagRepository = new TagRepository(configuration);
        _shareNoteRepository = new ShareNoteRepository(configuration);
        _noteTypeRepository = new NoteTypeRepository(configuration);
        _userRepository = new UserRepository(configuration);
        _authManager = new AuthManager(configuration);
        _userManager = new UserManager(configuration);
    }

    #region controller funcs (use in controllers)

    public async Task<IActionResult> Get(ClaimsPrincipal claims)
    {
        var user = await _userManager.GetUser(claims);
        
        var notesDomain = await GetNotes(user.Id);

        var notesView = notesDomain
            .Select(NoteViewBuilder.Create)
            .ToList();

        return new OkObjectResult(notesView);
    }

    public async Task<IActionResult> GetShared(ClaimsPrincipal claims)
    {
        var user = await _userManager.GetUser(claims);
        
        var notesDomain = await GetSharedNotes(user.Id);

        var notesView = notesDomain
            .Select(NoteViewBuilder.Create)
            .ToList();

        return new OkObjectResult(notesView);
    }

    public async Task<IActionResult> Get(ClaimsPrincipal claims, Guid id)
    {
        var user = await _userManager.GetUser(claims);
        
        var noteDomain = await GetNote(id);

        if (noteDomain == null)
            return new NotFoundResult();

        if (noteDomain.OwnerUser?.Email != user.Email)
            return new BadRequestObjectResult("Access denied");

        var noteView = NoteViewBuilder.Create(noteDomain);

        return new OkObjectResult(noteView);
    }

    // not save text and tags, only name and returns id
    public async Task<IActionResult> Create(ClaimsPrincipal claims, NoteBlank noteBlank)
    {
        var user = await _userManager.GetUser(claims);
        
        var noteDatabase = NoteDatabaseBuilder.Create(noteBlank, user.Id);

        noteDatabase.SourcePath = await NoteFileManager.CreateNoteText("");
        noteDatabase.CreationDate = DateTime.Now;
        noteDatabase.EditedDate = DateTime.Now;
        noteDatabase.Id = Guid.NewGuid();

        var result = await _noteRepository.Create(noteDatabase);

        return result != Guid.Empty ? new OkObjectResult(noteDatabase.Id) : new BadRequestResult();
    }

    public async Task<IActionResult> Update(ClaimsPrincipal claims, Guid guid, NoteBlank noteBlank)
    {
        var user = await _userManager.GetUser(claims);
        
        var noteDatabase = await _noteRepository.GetNote(guid);

        if (noteDatabase == null)
            return new NotFoundResult();

        var newNoteDatabase = NoteDatabaseBuilder.Create(noteBlank);

        if (noteBlank.Text != null)
        {
            if (noteDatabase.SourcePath != null)
            {
                await NoteFileManager.UpdateNoteText(noteDatabase.SourcePath, noteBlank.Text);
            }
            else
            {
                var path = await NoteFileManager.CreateNoteText(noteBlank.Text);

                newNoteDatabase.SourcePath = path;
            }
        }

        newNoteDatabase.EditedDate = DateTime.Now;

        await CreateNoteTags(guid, noteBlank.Tags);

        var result = await _noteRepository.Update(guid, newNoteDatabase);

        return result ? new OkResult() : new BadRequestResult();
    }

    public async Task<IActionResult> UploadImage(IFormFile formFile)
    {
        var fileName = Guid.NewGuid();

        var path = "wwwroot/" + fileName + ".png";

        await using StreamWriter sw = new StreamWriter(path);

        await formFile.CopyToAsync(sw.BaseStream);

        return new OkObjectResult(fileName);
    }

    public async Task<IActionResult> Share(ClaimsPrincipal claims, ShareBlank shareBlank)
    {
        var note = await GetNote(shareBlank.NoteId);

        if (note == null)
            return new NotFoundResult();

        var sharedUser = await _userRepository.Get(shareBlank.Email);

        if (sharedUser == null)
            return new NotFoundResult();

        await _noteRepository.UpdateType(note.Id, (int) NoteTypes.Public);

        var shareNote = new SharedNoteDatabase()
        {
            NoteId = shareBlank.NoteId,
            PermissionsLevelId = shareBlank.PermissionLevel,
            UserId = sharedUser.Id
        };

        var res = await _shareNoteRepository.Create(shareNote);

        return res > 0 ? new OkResult() : new BadRequestObjectResult("Invalid data");
    }

    public async Task<IActionResult> UpdateShare(ClaimsPrincipal claims, ShareBlank shareBlank)
    {
        var note = await GetNote(shareBlank.NoteId);

        if (note == null)
            return new NotFoundResult();

        var sharedUser = await _userRepository.Get(shareBlank.Email);

        if (sharedUser == null)
            return new NotFoundResult();

        var res = await _shareNoteRepository.Update(shareBlank.NoteId, sharedUser.Id, shareBlank.PermissionLevel);

        return res ? new OkResult() : new BadRequestObjectResult("Invalid data");
    }

    public async Task<IActionResult> DeleteShare(ClaimsPrincipal claims, Guid id, string email)
    {
        var note = await GetNote(id);

        if (note == null)
            return new NotFoundResult();

        var sharedUser = await _userRepository.Get(email);

        if (sharedUser == null)
            return new NotFoundResult();

        var res = await _shareNoteRepository.Delete(id, sharedUser.Id);

        return res ? new OkResult() : new BadRequestObjectResult("Error delete");
    }

    public async Task<IActionResult> Delete(ClaimsPrincipal claims, Guid id)
    {
        var user = await _userManager.GetUser(claims);
        
        var note = await _noteRepository.GetNote(id);

        if (note == null)
            return new NotFoundResult();

        if (note.OwnerId != user.Id)
            return new BadRequestObjectResult("Access denied");

        await _tagRepository.DeleteNoteTags(note.Id);
        await _shareNoteRepository.DeleteNoteShare(note.Id);

        var result = await _noteRepository.Delete(id);

        return result ? new OkResult() : new BadRequestObjectResult("Error delete");
    }

    #endregion

    #region get funcs
    
    private async Task<List<NoteDomain>> GetNotes(Guid userId)
    {
        var notesDatabase = await _noteRepository.GetNotes(userId);

        var notesDomain = new List<NoteDomain>();

        foreach (var note in notesDatabase)
            notesDomain.Add(await GetNoteDomain(note));

        return notesDomain;
    }

    private async Task<List<NoteDomain>> GetSharedNotes(Guid userId)
    {
        var notesDatabase = await _noteRepository.GetShared(userId);

        var notesDomain = new List<NoteDomain>();

        foreach (var note in notesDatabase)
            notesDomain.Add(await GetNoteDomain(note));

        return notesDomain;
    }

    private async Task<NoteDomain?> GetNote(Guid id)
    {
        var noteDatabase = await _noteRepository.GetNote(id);

        if (noteDatabase == null)
            return null;

        var noteDomain = await GetFullNoteDomain(noteDatabase);
        
        return noteDomain;
    }

    private async Task<List<UserDomain>> GetSharedUsers(Guid noteId)
    {
        var users = await _shareNoteRepository.GetSharedUsers(noteId);

        var usersDomain = users
            .Select(UserDomainBuilder.Create)
            .ToList();

        return usersDomain;
    }

    private async Task<NoteDomain> GetFullNoteDomain(NoteDatabase noteDatabase)
    {
        var noteDomain = NoteDomainBuilder.Create(noteDatabase);

        if (noteDatabase.SourcePath != null)
            noteDomain.Text = await NoteFileManager.GetNoteText(noteDatabase.SourcePath);

        var type = await _noteTypeRepository.Get(noteDomain.TypeId);

        var user = await _userRepository.Get(noteDomain.OwnerId);

        if (user != null)
            noteDomain.OwnerUser = UserDomainBuilder.Create(user);

        noteDomain.Type = NoteTypeDomainBuilder.Create(type);

        noteDomain.Tags = await GetNoteTags(noteDatabase.Id);

        noteDomain.SharedUsers = await GetSharedUsers(noteDomain.Id);

        return noteDomain;
    }
    
    private async Task<NoteDomain> GetNoteDomain(NoteDatabase noteDatabase)
    {
        var noteDomain = NoteDomainBuilder.Create(noteDatabase);

        var type = await _noteTypeRepository.Get(noteDomain.TypeId);

        var user = await _userRepository.Get(noteDomain.OwnerId);

        if (user != null)
            noteDomain.OwnerUser = UserDomainBuilder.Create(user);

        noteDomain.Type = NoteTypeDomainBuilder.Create(type);

        noteDomain.Tags = await GetNoteTags(noteDatabase.Id);
        
        return noteDomain;
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

    private async Task CreateNoteTags(Guid noteId, List<Guid> noteTags)
    {
        await _tagRepository.DeleteNoteTags(noteId);

        foreach (var noteTag in noteTags)
            await _tagRepository.Create(new NoteTagDatabase() {NoteId = noteId, TagId = noteTag});
    }

    #endregion
}