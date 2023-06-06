using Database.Note;
using Npgsql;

namespace Repositories.Repositories.Readers.Note;

public class NoteTypeReader : IReader<NoteTypeDatabase>
{
    private static readonly string _id = "id";
    private static readonly string _name = "name";
    
    public static async Task<NoteTypeDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            NoteTypeDatabase noteTypeDatabase = new NoteTypeDatabase();

            noteTypeDatabase.Id = reader.GetInt32(reader.GetOrdinal(_id));
            noteTypeDatabase.Name = reader.GetString(reader.GetOrdinal(_name));

            return noteTypeDatabase;
        }

        return null;
    }

    public static async Task<List<NoteTypeDatabase>> ReadListAsync(NpgsqlDataReader reader)
    {
        List<NoteTypeDatabase> noteDatabases = new List<NoteTypeDatabase>();

        while (await reader.ReadAsync())
        {
            NoteTypeDatabase noteTypeDatabase = new NoteTypeDatabase();

            noteTypeDatabase.Id = reader.GetInt32(reader.GetOrdinal(_id));
            noteTypeDatabase.Name = reader.GetString(reader.GetOrdinal(_name));
            
            noteDatabases.Add(noteTypeDatabase);
        }

        return noteDatabases;
    }
}