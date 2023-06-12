using Domain.Note.Tag;
using Views.Note.Tag;

namespace ViewBuilder.Note.Tag;

public static class TagViewBuilder
{
    public static TagView Create(TagDomain tagDomain)
    {
        return new TagView()
        {
            Id = tagDomain.Id,
            Name = tagDomain.Name
        };
    }

    public static TagView Create(Guid id, string name)
    {
        return new TagView()
        {
            Id = id,
            Name = name
        };
    }
}