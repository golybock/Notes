using Database.Note.Tag;
using Npgsql;

namespace NotesApi.Repositories.Readers.Note.Tag;

public class TagReader : IReader<TagDatabase>
{
    private const string Id = "id";
    private const string Name = "name";
    
    public static async Task<TagDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        TagDatabase tagDatabase = new TagDatabase();
        
        while (await reader.ReadAsync())
        {
            tagDatabase.Id = reader.GetInt32(reader.GetOrdinal(Id));
            tagDatabase.Name = reader.GetString(reader.GetOrdinal(Name));
        }
        
        return tagDatabase;
    }

    public static async Task<List<TagDatabase>> ReadListAsync(NpgsqlDataReader reader)
    {
        List<TagDatabase> tagDatabases = new List<TagDatabase>();
        
        while (await reader.ReadAsync())
        {
            TagDatabase tagDatabase = new TagDatabase();
            
            tagDatabase.Id = reader.GetInt32(reader.GetOrdinal(Id));
            tagDatabase.Name = reader.GetString(reader.GetOrdinal(Name));
            
            tagDatabases.Add(tagDatabase);
        }
        
        return tagDatabases;
    }
}