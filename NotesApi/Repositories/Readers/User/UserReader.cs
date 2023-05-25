using Database.User;
using Npgsql;

namespace NotesApi.Repositories.Readers.User;

public class UserReader : IReader<NoteUserDatabase>
{
    public static async Task<NoteUserDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            NoteUserDatabase noteUserDatabase = new NoteUserDatabase();
            
            noteUserDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
            noteUserDatabase.Email = reader.GetString(reader.GetOrdinal("email"));
                
            var passwordHash = reader.GetValue(reader.GetOrdinal("password_hash"));

            if (passwordHash != DBNull.Value)
                noteUserDatabase.PasswordHash = passwordHash.ToString();
            
            return noteUserDatabase;
        }

        return null;
    }

    public static async Task<List<NoteUserDatabase>> ReadListAsync(NpgsqlDataReader reader)
    {
        List<NoteUserDatabase> userDatabases = new List<NoteUserDatabase>();

        while (await reader.ReadAsync())
        {
            NoteUserDatabase noteUserDatabase = new NoteUserDatabase();
            
            noteUserDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
            noteUserDatabase.Email = reader.GetString(reader.GetOrdinal("email"));
                
            var passwordHash = reader.GetValue(reader.GetOrdinal("password_hash"));

            if (passwordHash != DBNull.Value)
                noteUserDatabase.PasswordHash = passwordHash.ToString();
        }

        return userDatabases;
    }
}