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
using NotesApi.Services.Note.NoteFileManager;
using NotesApi.Services.User.UserManager;
using Repositories.Repositories.Note;
using Repositories.Repositories.Note.NoteImage;
using Repositories.Repositories.Note.NoteType;
using Repositories.Repositories.Note.ShareNote;
using Repositories.Repositories.Note.Tag;
using ViewBuilder.Note;

namespace NotesApi.Services.Note;

// todo refactor
public class NoteService : INoteService
{
    private readonly IShareNotesRepository _shareNoteRepository;
    private readonly INoteImageRepository _noteImageRepository;
    private readonly INoteTypeRepository _noteTypeRepository;
    private readonly INoteRepository _noteRepository;
    private readonly IUserManager _userManager;
    private readonly ITagRepository _tagRepository;
    private readonly INoteFileManager _noteFileManager;

    public NoteService(
        ITagRepository tagRepository,
        IUserManager userManager,
        INoteRepository noteRepository,
        INoteTypeRepository noteTypeRepository,
        INoteImageRepository noteImageRepository,
        IShareNotesRepository shareNoteRepository,
        INoteFileManager noteFileManager
    )
    {
        _tagRepository = tagRepository;
        _userManager = userManager;
        _noteRepository = noteRepository;
        _noteTypeRepository = noteTypeRepository;
        _noteImageRepository = noteImageRepository;
        _shareNoteRepository = shareNoteRepository;
        _noteFileManager = noteFileManager;
    }

    #region main funcs (IActionResult)

    public async Task<IActionResult> Get(ClaimsPrincipal claims)
    {
        var user = await _userManager.Get(claims);

        var notesDomain = await GetNotes(user.Id);

        var notesView = notesDomain
            .Select(NoteViewBuilder.Create)
            .ToList();

        return new OkObjectResult(notesView);
    }

    public async Task<IActionResult> GetShared(ClaimsPrincipal claims)
    {
        var user = await _userManager.Get(claims);

        var notesDomain = await GetSharedNotes(user.Id);

        var notesView = notesDomain
            .Select(NoteViewBuilder.Create)
            .ToList();

        return new OkObjectResult(notesView);
    }

    public async Task<IActionResult> Get(ClaimsPrincipal claims, Guid id)
    {
        var user = await _userManager.Get(claims);

        var noteDomain = await GetNote(id, user.Id);

        if (noteDomain == null)
            return new NotFoundResult();

        var noteView = NoteViewBuilder.Create(noteDomain);

        return new OkObjectResult(noteView);
    }

    // only create note id db and returns id
    public async Task<IActionResult> Create(ClaimsPrincipal claims, NoteBlank noteBlank)
    {
        var id = Guid.NewGuid();

        var user = await _userManager.Get(claims);

        var noteDatabase = NoteDatabaseBuilder.Create(id, noteBlank, user.Id);

        noteDatabase.CreationDate = DateTime.UtcNow;
        noteDatabase.EditedDate = DateTime.UtcNow;

        var result = await _noteRepository.Create(noteDatabase);

        return result != Guid.Empty ? new OkObjectResult(noteDatabase.Id) : new BadRequestResult();
    }

    // todo update security level
    public async Task<IActionResult> Update(ClaimsPrincipal claims, Guid guid, NoteBlank noteBlank)
    {
        var user = await _userManager.Get(claims);

        var noteDatabase = await GetNoteDatabase(guid, user.Id);

        if (noteDatabase == null)
            return new NotFoundResult();

        var newNoteDatabase = NoteDatabaseBuilder.Create(noteBlank);

        await _noteFileManager.SetNoteText(noteDatabase.Id, noteBlank.Text ?? string.Empty);

        // todo scope to func (images)
        await _noteImageRepository.Clear(guid);

        foreach (var image in noteBlank.Images)
            await CreateOrUpdateImage(image, guid);
        
        await CreateNoteTags(guid, noteBlank.Tags);
        
        newNoteDatabase.EditedDate = DateTime.UtcNow;

        var result = await _noteRepository.Update(guid, newNoteDatabase);

        return result ? new OkResult() : new BadRequestResult();
    }

    // todo refactor to svg (add mode)
    public async Task<IActionResult> UploadImage(IFormFile formFile, Guid noteId)
    {
        var format = Path.GetExtension(formFile.FileName);

        if (format != ".svg" || format != ".png")
            return new BadRequestObjectResult("Only images available");
        
        var fileName = Guid.NewGuid();

        var path = "wwwroot/" + fileName + format;

        await using StreamWriter sw = new StreamWriter(path);

        var imageDatabase = NoteImageDatabaseBuilder.CreateDefault(fileName, noteId);

        await _noteImageRepository.Create(imageDatabase);

        await formFile.CopyToAsync(sw.BaseStream);

        return new OkObjectResult(fileName);
    }

    public async Task<IActionResult> Share(ClaimsPrincipal claims, ShareBlank shareBlank)
    {
        var user = await _userManager.Get(claims);

        var note = await GetNoteDatabase(shareBlank.NoteId, user.Id);

        if (note == null)
            return new NotFoundResult();

        if (user.Id != note.OwnerId)
            return new BadRequestObjectResult("Access denied");

        var sharedUser = await _userManager.Get(shareBlank.Email);

        if (sharedUser == null)
            return new NotFoundResult();

        await _noteRepository.UpdateType(note.Id, (int) NoteType.Public);

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
        var user = await _userManager.Get(claims);

        var note = await GetNoteDatabase(shareBlank.NoteId, user.Id);

        if (note == null)
            return new NotFoundResult();

        if (user.Id != note.OwnerId)
            return new BadRequestObjectResult("Access denied");

        var sharedUser = await _userManager.Get(shareBlank.Email);

        if (sharedUser == null)
            return new NotFoundResult();

        var res = await _shareNoteRepository.Update(shareBlank.NoteId, sharedUser.Id, shareBlank.PermissionLevel);

        return res ? new OkResult() : new BadRequestObjectResult("Invalid data");
    }

    public async Task<IActionResult> DeleteShare(ClaimsPrincipal claims, Guid id, string email)
    {
        var user = await _userManager.Get(claims);

        var note = await GetNoteDatabase(id, user.Id);

        if (note == null)
            return new NotFoundResult();

        // only owner can edit shared users list 
        if (user.Id != note.OwnerId)
            return new BadRequestObjectResult("Access denied");

        var sharedUser = await _userManager.Get(email);

        if (sharedUser == null)
            return new NotFoundObjectResult("User not found");

        var res = await _shareNoteRepository.Delete(id, sharedUser.Id);

        return res ? new OkResult() : new BadRequestObjectResult("Error delete");
    }

    // todo maybe refactor to set type 'deleted' and add to trash
    public async Task<IActionResult> Delete(ClaimsPrincipal claims, Guid id)
    {
        var user = await _userManager.Get(claims);

        var note = await _noteRepository.GetNote(id, user.Id);

        if (note == null)
            return new NotFoundResult();

        // only owner can delete
        if (note.OwnerId != user.Id)
            return new BadRequestObjectResult("Access denied");

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

    private async Task<NoteDomain?> GetNote(Guid id, Guid userId)
    {
        var noteDatabase = await _noteRepository.GetNote(id, userId);

        if (noteDatabase == null)
            return null;

        var noteDomain = await GetFullNoteDomain(noteDatabase);

        return noteDomain;
    }
    
    private async Task<NoteDatabase?> GetNoteDatabase(Guid id, Guid userId)
    {
        var noteDatabase = await _noteRepository.GetNote(id, userId);
        
        return noteDatabase;
    }

    private async Task<List<UserDomain>> GetSharedUsers(Guid noteId)
    {
        var users = await _shareNoteRepository.GetSharedUsers(noteId);

        var usersDomain = users
            .Select(UserDomainBuilder.Create)
            .ToList();

        return usersDomain;
    }

    // todo refactor
    private async Task<NoteDomain> GetFullNoteDomain(NoteDatabase noteDatabase)
    {
        var text = await _noteFileManager.GetNoteText(noteDatabase.Id);

        var type = await _noteTypeRepository.Get(noteDatabase.TypeId);

        var user = await _userManager.Get(noteDatabase.OwnerId);

        var noteDomain = NoteDomainBuilder.Create(noteDatabase, type, text);

        noteDomain.Tags = await GetNoteTags(noteDatabase.Id);

        noteDomain.Images = await GetNoteImagesDomain(noteDatabase.Id);

        noteDomain.SharedUsers = await GetSharedUsers(noteDomain.Id);

        if (user != null)
            noteDomain.OwnerUser = user;

        return noteDomain;
    }

    private async Task<NoteDomain> GetNoteDomain(NoteDatabase noteDatabase)
    {
        var type = await _noteTypeRepository.Get(noteDatabase.TypeId);

        var user = await _userManager.Get(noteDatabase.OwnerId);

        var noteDomain = NoteDomainBuilder.Create(noteDatabase, type);

        if (user != null)
            noteDomain.OwnerUser = user;

        noteDomain.Tags = await GetNoteTags(noteDatabase.Id);

        return noteDomain;
    }

    private async Task<List<NoteImageDomain>> GetNoteImagesDomain(Guid noteId)
    {
        var images = await _noteImageRepository.GetImages(noteId);

        var imagesDomain = images.Select(NoteImageDomainBuilder.Create).ToList();

        return imagesDomain;
    }

    #endregion

    private async Task CreateOrUpdateImage(NoteImageBlank noteImageBlank, Guid noteId)
    {
        var db = NoteImageDatabaseBuilder.Create(noteImageBlank, noteId);

        await _noteImageRepository.Create(db);
    }

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
        // clear tags
        await _tagRepository.DeleteNoteTags(noteId);

        // add tags
        foreach (var noteTag in noteTags)
            await _tagRepository.Create(new NoteTagDatabase() {NoteId = noteId, TagId = noteTag});
    }

    #endregion
}