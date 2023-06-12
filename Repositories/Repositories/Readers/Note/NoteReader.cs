using Database.Note;
using Npgsql;

namespace Repositories.Repositories.Readers.Note;

public class NoteReader : IReader<NoteDatabase>
{
    private const string Id = "id";
    private const string OwnerId = "owner_id";
    private const string Header = "header";
    private const string CreatedDate = "created_date";
    private const string SourcePath = "source_path";
    private const string TypeId = "type_id";
    private const string EditedDate = "edited_date";
    
    public static async Task<NoteDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            NoteDatabase noteDatabase = new NoteDatabase();
            
            noteDatabase.Id = reader.GetGuid(reader.GetOrdinal(Id));
            noteDatabase.OwnerId = reader.GetGuid(reader.GetOrdinal(OwnerId));
            noteDatabase.Header = reader.GetString(reader.GetOrdinal(Header));
            noteDatabase.CreationDate = reader.GetDateTime(reader.GetOrdinal(CreatedDate));
            noteDatabase.EditedDate = reader.GetDateTime(reader.GetOrdinal(EditedDate));
            noteDatabase.TypeId = reader.GetInt32(reader.GetOrdinal(TypeId));
            
            var sourcePath = reader.GetValue(reader.GetOrdinal(SourcePath));

            if (sourcePath != DBNull.Value)
                noteDatabase.SourcePath = sourcePath.ToString();
            
            return noteDatabase;
        }

        return null;
    }

    public static async Task<List<NoteDatabase>> ReadListAsync(NpgsqlDataReader reader)
    {
        List<NoteDatabase> noteDatabases = new List<NoteDatabase>();

        while (await reader.ReadAsync())
        {
            NoteDatabase noteDatabase = new NoteDatabase();

            noteDatabase.Id = reader.GetGuid(reader.GetOrdinal(Id));
            noteDatabase.OwnerId = reader.GetGuid(reader.GetOrdinal(OwnerId));
            noteDatabase.Header = reader.GetString(reader.GetOrdinal(Header));
            noteDatabase.CreationDate = reader.GetDateTime(reader.GetOrdinal(CreatedDate));
            noteDatabase.EditedDate = reader.GetDateTime(reader.GetOrdinal(EditedDate));
            noteDatabase.TypeId = reader.GetInt32(reader.GetOrdinal(TypeId));
            
            var sourcePath = reader.GetValue(reader.GetOrdinal(SourcePath));

            if (sourcePath != DBNull.Value)
                noteDatabase.SourcePath = sourcePath.ToString();
            
            noteDatabases.Add(noteDatabase);
        }

        return noteDatabases;
    }
}