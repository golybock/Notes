using Database.Note.Tag;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Npgsql;

namespace NotesApi.Repositories.Readers.Note.Tag;

public class NoteTagReader : IReader<NoteTagDatabase>
{
    private const string Id = "id";
    private const string NoteId = "note_id";
    private const string TagId = "tag_id";

    public static async Task<NoteTagDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            NoteTagDatabase noteTagDatabase = new NoteTagDatabase();
            
            noteTagDatabase.Id = reader.GetInt32(reader.GetOrdinal(Id));
            noteTagDatabase.NoteId = reader.GetInt32(reader.GetOrdinal(NoteId));
            noteTagDatabase.TagId = reader.GetInt32(reader.GetOrdinal(TagId));
                
            return noteTagDatabase;
        }

        return null;
    }
    
    public static async Task<List<NoteTagDatabase>> ReadListAsync(NpgsqlDataReader reader)
    {
        List<NoteTagDatabase> noteTagDatabases = new List<NoteTagDatabase>();

        while (await reader.ReadAsync())
        {
            NoteTagDatabase noteTagDatabase = new NoteTagDatabase();
            
            noteTagDatabase.Id = reader.GetInt32(reader.GetOrdinal(Id));
            noteTagDatabase.NoteId = reader.GetInt32(reader.GetOrdinal(NoteId));
            noteTagDatabase.TagId = reader.GetInt32(reader.GetOrdinal(TagId));
                
            noteTagDatabases.Add(noteTagDatabase);
        }

        return noteTagDatabases;
    }
}