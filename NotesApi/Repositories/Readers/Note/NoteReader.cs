using Database.Note;
using Npgsql;

namespace NotesApi.Repositories.Readers.Note;

public class NoteReader : IReader<NoteDatabase>
{
    public static async Task<NoteDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            NoteDatabase noteDatabase = new NoteDatabase();
            
            noteDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
            noteDatabase.UserId = reader.GetInt32(reader.GetOrdinal("user_id"));
            noteDatabase.Header = reader.GetString(reader.GetOrdinal("header"));
            noteDatabase.CreationDate = reader.GetDateTime(reader.GetOrdinal("creation_date"));
            noteDatabase.EditedDate = reader.GetDateTime(reader.GetOrdinal("edited_date"));
            noteDatabase.Guid = reader.GetGuid(reader.GetOrdinal("guid"));

            var sourcePath = reader.GetValue(reader.GetOrdinal("source_path"));

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

            noteDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
            noteDatabase.UserId = reader.GetInt32(reader.GetOrdinal("user_id"));
            noteDatabase.Header = reader.GetString(reader.GetOrdinal("header"));
            noteDatabase.CreationDate = reader.GetDateTime(reader.GetOrdinal("creation_date"));
            noteDatabase.EditedDate = reader.GetDateTime(reader.GetOrdinal("edited_date"));
            noteDatabase.Guid = reader.GetGuid(reader.GetOrdinal("guid"));

            var sourcePath = reader.GetValue(reader.GetOrdinal("source_path"));

            if (sourcePath != DBNull.Value)
                noteDatabase.SourcePath = sourcePath.ToString();
            
            noteDatabases.Add(noteDatabase);
        }

        return noteDatabases;
    }
}