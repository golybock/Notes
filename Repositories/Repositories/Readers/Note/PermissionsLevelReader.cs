using Database.Note;
using Npgsql;

namespace Repositories.Repositories.Readers.Note;

public class PermissionsLevelReader : IReader<PermissionsLevelDatabase>
{
    private static readonly string Id = "id";
    private static readonly string Name = "name";
    
    public static async Task<PermissionsLevelDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            PermissionsLevelDatabase permissionsLevelDatabase = new PermissionsLevelDatabase();

            permissionsLevelDatabase.Id = reader.GetInt32(reader.GetOrdinal(Id));
            permissionsLevelDatabase.Name = reader.GetString(reader.GetOrdinal(Name));
                
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

            permissionLevel.Id = reader.GetInt32(reader.GetOrdinal(Id));
            permissionLevel.Name = reader.GetString(reader.GetOrdinal(Name));

            
            permissionsLevel.Add(permissionLevel);
        }

        return permissionsLevel;
    }
}