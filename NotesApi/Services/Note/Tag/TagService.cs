using Blank.Note.Tag;
using DatabaseBuilder.Note.Tag;
using DomainBuilder.Note.Tag;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Services.Interfaces.Note.Tag;
using Repositories.Repositories.Note.Tag;
using ViewBuilder.Note.Tag;
using Views.Note.Tag;

namespace NotesApi.Services.Note.Tag;

public class TagService : ITagService
{
    private readonly TagRepository _tagRepository;

    public TagService(IConfiguration configuration)
    {
        _tagRepository = new TagRepository(configuration);
    }

    public async Task<IActionResult> Get()
    {
        var databaseTags = await _tagRepository.Get();

        var domainTags = databaseTags
            .Select(TagDomainBuilder.Create)
            .ToList();

        var viewTags = domainTags
            .Select(TagViewBuilder.Create)
            .ToList();

        return new OkObjectResult(viewTags);
    }

    public async Task<IActionResult> Get(Guid id)
    {
        if (id == Guid.Empty)
            return new BadRequestObjectResult("Not valid id");
        
        var databaseTag = await _tagRepository.Get(id);

        if (databaseTag == null)
            return new NotFoundResult();
        
        var domainTag = TagDomainBuilder.Create(databaseTag);

        var viewTag = TagViewBuilder.Create(domainTag);

        return new OkObjectResult(viewTag);
    }

    public async Task<IActionResult> GetByNote(Guid noteId)
    {
        if (noteId == Guid.Empty)
            return new BadRequestObjectResult("Not valid id");
        
        var databaseTags = await _tagRepository.GetNoteTags(noteId);

        var domainTags = databaseTags
            .Select(TagDomainBuilder.Create)
            .ToList();

        var viewTags = domainTags
            .Select(TagViewBuilder.Create)
            .ToList();

        return new OkObjectResult(viewTags);
    }

    public async Task<IActionResult> Create(TagBlank tagBlank)
    {
        if (string.IsNullOrEmpty(tagBlank.Name))
            return new BadRequestObjectResult("Name not can be empty");

        var tagDatabase = TagDatabaseBuilder.Create(tagBlank);
        
        try
        {
            var result = await _tagRepository.Create(tagDatabase);
            return result > 0 ? new OkObjectResult(result) : new BadRequestResult();
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult("tag already created");
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _tagRepository.Delete(id);
        
        return result ? new OkResult() : new BadRequestResult();
    }
}