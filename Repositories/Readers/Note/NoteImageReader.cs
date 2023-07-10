using Database.Note;
using Npgsql;

namespace Repositories.Readers.Note;

public class NoteImageReader : IReader<NoteImageDatabase>
{
    private const string Id = "id";
    private const string NoteId = "note_id";
    private const string X = "x";
    private const string Y = "y";
    private const string Width = "width";
    private const string Height = "height";

    public static async Task<NoteImageDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            NoteImageDatabase noteImageDatabase = new NoteImageDatabase();
            
            noteImageDatabase.Id = reader.GetGuid(reader.GetOrdinal(Id));
            noteImageDatabase.NoteId = reader.GetGuid(reader.GetOrdinal(NoteId));
            noteImageDatabase.X = reader.GetInt32(reader.GetOrdinal(X));
            noteImageDatabase.Y = reader.GetInt32(reader.GetOrdinal(Y));
            noteImageDatabase.Width = reader.GetInt32(reader.GetOrdinal(Width));
            noteImageDatabase.Height = reader.GetInt32(reader.GetOrdinal(Height));

            return noteImageDatabase;
        }

        return null;
    }

    public static async Task<List<NoteImageDatabase>> ReadListAsync(NpgsqlDataReader reader)
    {
        List<NoteImageDatabase> noteImageDatabases = new List<NoteImageDatabase>();

        while (await reader.ReadAsync())
        {
            NoteImageDatabase noteImageDatabase = new NoteImageDatabase();
            
            noteImageDatabase.Id = reader.GetGuid(reader.GetOrdinal(Id));
            noteImageDatabase.NoteId = reader.GetGuid(reader.GetOrdinal(NoteId));
            noteImageDatabase.X = reader.GetInt32(reader.GetOrdinal(X));
            noteImageDatabase.Y = reader.GetInt32(reader.GetOrdinal(Y));
            noteImageDatabase.Width = reader.GetInt32(reader.GetOrdinal(Width));
            noteImageDatabase.Height = reader.GetInt32(reader.GetOrdinal(Height));

            noteImageDatabases.Add(noteImageDatabase);
        }

        return noteImageDatabases;
    }
}