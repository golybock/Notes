using Database.Note;
using Npgsql;

namespace NotesApi.Repositories.Readers.Note;

public class PermissionsLevelReader : IReader<PermissionsLevelDatabase>
{
    private static readonly string _id = "id";
    private static readonly string _name = "name";
    
    public static async Task<PermissionsLevelDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            PermissionsLevelDatabase permissionsLevelDatabase = new PermissionsLevelDatabase();

            permissionsLevelDatabase.Id = reader.GetInt32(reader.GetOrdinal(_id));
            permissionsLevelDatabase.Name = reader.GetString(reader.GetOrdinal(_name));
                
            return permissionsLevelDatabase;
        }

        return null;
    }

    public static async Task<List<PermissionsLevelDatabase>> ReadListAsync(NpgsqlDataReader reader)
    {
        List<PermissionsLevelDatabase> permissionsLevel = new List<PermissionsLevelDatabase>();

        while (await reader.ReadAsync())
        {
            PermissionsLevelDatabase permissionLevel = new PermissionsLevelDatabase();

            permissionLevel.Id = reader.GetInt32(reader.GetOrdinal(_id));
            permissionLevel.Name = reader.GetString(reader.GetOrdinal(_name));

            
            permissionsLevel.Add(permissionLevel);
        }

        return permissionsLevel;
    }
}