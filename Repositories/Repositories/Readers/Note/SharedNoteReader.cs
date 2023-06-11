using Database.Note;
using Database.User;
using Npgsql;

namespace Repositories.Repositories.Readers.Note;

public class SharedNoteReader : IReader<SharedNoteDatabase>
{
    public static async Task<SharedNoteDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            SharedNoteDatabase sharedNoteDatabase = new SharedNoteDatabase();

            sharedNoteDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
            sharedNoteDatabase.NoteId = reader.GetGuid(reader.GetOrdinal("note_id"));
            sharedNoteDatabase.UserId = reader.GetInt32(reader.GetOrdinal("user_id"));
            sharedNoteDatabase.PermissionsLevelId = reader.GetInt32(reader.GetOrdinal("permissions_level_id"));
            
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

            sharedNoteDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
            sharedNoteDatabase.NoteId = reader.GetGuid(reader.GetOrdinal("note_id"));
            sharedNoteDatabase.UserId = reader.GetInt32(reader.GetOrdinal("user_id"));
            sharedNoteDatabase.PermissionsLevelId = reader.GetInt32(reader.GetOrdinal("permissions_level_id"));
            
            sharedNoteDatabases.Add(sharedNoteDatabase);
        }

        return sharedNoteDatabases;
    }
    
    public static async Task<List<UserDatabase>> ReadUsersAsync(NpgsqlDataReader reader)
    {
        List<UserDatabase> users = new List<UserDatabase>();

        while (await reader.ReadAsync())
        {
            UserDatabase userDatabase = new UserDatabase();

            userDatabase.Id = reader.GetInt32(reader.GetOrdinal("u.id"));
            userDatabase.Email = reader.GetString(reader.GetOrdinal("u.email"));
            userDatabase.Name = reader.GetString(reader.GetOrdinal("u.name"));
            
            users.Add(userDatabase);
        }

        return users;
    }
}