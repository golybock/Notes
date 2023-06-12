using Database.Note.Tag;
using Domain.Note.Tag;

namespace DomainBuilder.Note.Tag;

public static class TagDomainBuilder
{
    public static TagDomain Create(TagDatabase tagDatabase)
    {
        return new TagDomain()
        {
            Id = tagDatabase.Id,
            Name = tagDatabase.Name
        };
    }
    
    public static TagDomain Create(Guid id, TagDatabase tagDatabase)
    {
        return new TagDomain()
        {
            Id = id,
            Name = tagDatabase.Name
        };
    }
    
    public static TagDomain Create(Guid id, string name)
    {
        return new TagDomain()
        {
            Id = id,
            Name = name
        };
    }
    
    public static TagDomain Create(string name)
    {
        return new TagDomain()
        {
            Name = name
        };
    }
}