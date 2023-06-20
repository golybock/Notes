using Database.Note;
using Npgsql;

namespace Repositories.Readers.Note;

public class SharedNoteReader : IReader<SharedNoteDatabase>
{
    private const string Id = "id";
    private const string NoteId = "note_id";
    private const string UserId = "user_id";
    private const string PermissionsLevelId = "permissions_level_id";
    
    public static async Task<SharedNoteDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            SharedNoteDatabase sharedNoteDatabase = new SharedNoteDatabase();

            sharedNoteDatabase.Id = reader.GetInt32(reader.GetOrdinal(Id));
            sharedNoteDatabase.NoteId = reader.GetGuid(reader.GetOrdinal(NoteId));
            sharedNoteDatabase.UserId = reader.GetGuid(reader.GetOrdinal(UserId));
            sharedNoteDatabase.PermissionsLevelId = reader.GetInt32(reader.GetOrdinal(PermissionsLevelId));
            
            return sharedNoteDatabase;
        }

        return null;
    }

    public static async Task<List<SharedNoteDatabase>> ReadListAsync(NpgsqlDataReader reader)
    {
        List<SharedNoteDatabase> sharedNoteDatabases = new List<SharedNoteDatabase>();

        while (await reader.ReadAsync())
        {
            SharedNoteDatabase sharedNoteDatabase = new SharedNoteDatabase();

            sharedNoteDatabase.Id = reader.GetInt32(reader.GetOrdinal(Id));
            sharedNoteDatabase.NoteId = reader.GetGuid(reader.GetOrdinal(NoteId));
            sharedNoteDatabase.UserId = reader.GetGuid(reader.GetOrdinal(UserId));
            sharedNoteDatabase.PermissionsLevelId = reader.GetInt32(reader.GetOrdinal(PermissionsLevelId));
            
            sharedNoteDatabases.Add(sharedNoteDatabase);
        }

        return sharedNoteDatabases;
    }
}