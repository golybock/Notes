using Database.Note;
using Npgsql;

namespace Repositories.Readers.Note;

public class NoteTypeReader : IReader<NoteTypeDatabase>
{
    private const string Id = "id";
    private const string Name = "name";

    public static async Task<NoteTypeDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            NoteTypeDatabase noteTypeDatabase = new NoteTypeDatabase();

            noteTypeDatabase.Id = reader.GetInt32(reader.GetOrdinal(Id));
            noteTypeDatabase.Name = reader.GetString(reader.GetOrdinal(Name));

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

            noteTypeDatabase.Id = reader.GetInt32(reader.GetOrdinal(Id));
            noteTypeDatabase.Name = reader.GetString(reader.GetOrdinal(Name));
            
            noteDatabases.Add(noteTypeDatabase);
        }

        return noteDatabases;
    }
}