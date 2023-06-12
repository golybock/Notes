using Blank.Note.Tag;
using Database.Note.Tag;

namespace DatabaseBuilder.Note.Tag;

public static class TagDatabaseBuilder
{
    public static TagDatabase Create(Guid id, TagBlank tagBlank) =>
        new() {Id = id, Name = tagBlank.Name};

    public static TagDatabase Create(TagBlank tagBlank) =>
        new() {Name = tagBlank.Name};

    public static TagDatabase Create(string name) =>
        new() {Name = name};
}