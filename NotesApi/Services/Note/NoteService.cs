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
    private readonly ShareNoteRepository _shareNoteRepository;
    private readonly NoteTypeRepository _noteTypeRepository;
    private readonly UserRepository _userRepository;

    public NoteService(IConfiguration configuration)
    {
        _noteRepository = new NoteRepository(configuration);
        _tagRepository = new TagRepository(configuration);
        _shareNoteRepository = new ShareNoteRepository(configuration);
        _noteTypeRepository = new NoteTypeRepository(configuration);
        _userRepository = new UserRepository(configuration);
    }

    #region controller funcs (use in controllers)

    public async Task<IActionResult> Get(UserDomain user)
    {
        var notesDomain = await GetNotes(user.Id);

        var notesView = notesDomain
            .Select(NoteViewBuilder.Create)
            .ToList();

        return new OkObjectResult(notesView);
    }

    public async Task<IActionResult> GetShared(UserDomain user)
    {
        var notesDomain = await GetSharedNotes(user.Id);

        var notesView = notesDomain
            .Select(NoteViewBuilder.Create)
            .ToList();

        return new OkObjectResult(notesView);
    }

    public async Task<IActionResult> Get(UserDomain user, Guid id)
    {
        var noteDomain = await GetNote(id);

        if (noteDomain == null)
            return new NotFoundResult();

        if (noteDomain.OwnerUser?.Email != user.Email)
            return new BadRequestObjectResult("Access denied");

        var noteView = NoteViewBuilder.Create(noteDomain);

        return new OkObjectResult(noteView);
    }

    // not save text and tags, only name and returns id
    public async Task<IActionResult> Create(UserDomain user, NoteBlank noteBlank)
    {
        var noteDatabase = NoteDatabaseBuilder.Create(noteBlank, user.Id);

        noteDatabase.SourcePath = await NoteFileManager.CreateNoteText("");
        noteDatabase.CreationDate = DateTime.Now;
        noteDatabase.EditedDate = DateTime.Now;
        noteDatabase.Id = Guid.NewGuid();

        var result = await _noteRepository.Create(noteDatabase);

        return result != Guid.Empty ? new OkObjectResult(noteDatabase.Id) : new BadRequestResult();
    }

    public async Task<IActionResult> Update(UserDomain user, Guid guid, NoteBlank noteBlank)
    {
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

        await CreateNoteTags(newNoteDatabase.Id, noteBlank.Tags);

        var result = await _noteRepository.Update(guid, newNoteDatabase);

        return result ? new OkResult() : new BadRequestResult();
    }

    public async Task<IActionResult> Share(UserDomain user, ShareBlank shareBlank)
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

    public async Task<IActionResult> UpdateShare(UserDomain user, ShareBlank shareBlank)
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

    public async Task<IActionResult> DeleteShare(UserDomain user, Guid id, string email)
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

    public async Task<IActionResult> Delete(UserDomain user, Guid id)
    {
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

        var notesDomain = notesDatabase
            .Select(NoteDomainBuilder.Create)
            .ToList();

        foreach (var note in notesDomain)
        {
            var type = await _noteTypeRepository.Get(note.TypeId);

            note.Type = NoteTypeDomainBuilder.Create(type);

            note.Tags = await GetNoteTags(note.Id);

            if (note.SourcePath != null)
                note.Text = await NoteFileManager.GetNoteText(note.SourcePath);

            note.SharedUsers = await GetSharedUsers(note.Id);

            note.OwnerUser = await GetUser(note.OwnerId);
        }

        return notesDomain;
    }

    private async Task<List<NoteDomain>> GetSharedNotes(Guid userId)
    {
        var notesDatabase = await _noteRepository.GetShared(userId);

        var notesDomain = notesDatabase
            .Select(NoteDomainBuilder.Create)
            .ToList();

        foreach (var note in notesDomain)
        {
            var type = await _noteTypeRepository.Get(note.TypeId);

            note.Tags = await GetNoteTags(note.Id);

            note.Type = NoteTypeDomainBuilder.Create(type);
            
            note.OwnerUser = await GetUser(note.OwnerId);

            note.SharedUsers = await GetSharedUsers(note.Id);
            
            if (note.SourcePath != null)
                note.Text = await NoteFileManager.GetNoteText(note.SourcePath);
        }

        return notesDomain;
    }

    private async Task<NoteDomain?> GetNote(Guid id)
    {
        var noteDatabase = await _noteRepository.GetNote(id);

        if (noteDatabase == null)
            return null;

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

    private async Task<List<UserDomain>> GetSharedUsers(Guid noteId)
    {
        var users = await _shareNoteRepository.GetSharedUsers(noteId);

        var usersDomain = users
            .Select(UserDomainBuilder.Create)
            .ToList();

        return usersDomain;
    }

    private async Task<UserDomain?> GetUser(string email)
    {
        var user = await _userRepository.Get(email);

        if (user == null)
            return null;

        return UserDomainBuilder.Create(user);
    }

    private async Task<UserDomain?> GetUser(Guid id)
    {
        var user = await _userRepository.Get(id);

        if (user == null)
            return null;

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

    private async Task CreateNoteTags(Guid noteId, List<Guid> noteTags)
    {
        await _tagRepository.DeleteNoteTags(noteId);

        foreach (var noteTagId in noteTags)
            await _tagRepository.Create(new NoteTagDatabase() {NoteId = noteId, TagId = noteTagId});
    }

    #endregion
}