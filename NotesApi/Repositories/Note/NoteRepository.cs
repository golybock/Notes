using Database.Note;
using NotesApi.Repositories.Interfaces.Note;
using Npgsql;

namespace NotesApi.Repositories.Note;

public class NoteRepository : RepositoryBase, INoteRepository
{
    public NoteRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<NoteDatabase?> Get(int id)
    {
        NoteDatabase noteTagDatabase = new NoteDatabase();

        string query = "select * from note where id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = { new NpgsqlParameter(){ Value = id} }
            };

            await using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                noteTagDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
                noteTagDatabase.UserId = reader.GetInt32(reader.GetOrdinal("user_id"));
                noteTagDatabase.Header = reader.GetString(reader.GetOrdinal("header"));
                noteTagDatabase.CreationDate = reader.GetDateTime(reader.GetOrdinal("creation_date"));
                noteTagDatabase.LastEditDate = reader.GetDateTime(reader.GetOrdinal("last_edit_date"));

                var sourcePath = reader.GetValue(reader.GetOrdinal("source_path"));

                if (sourcePath != DBNull.Value)
                    noteTagDatabase.SourcePath = sourcePath.ToString();
                
                return noteTagDatabase;
            }

            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<List<NoteDatabase>> Get()
    {
        List<NoteDatabase> noteTagDatabases = new List<NoteDatabase>();

        string query = "select * from note";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection);

            await using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                NoteDatabase noteTagDatabase = new NoteDatabase();
                
                noteTagDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
                noteTagDatabase.UserId = reader.GetInt32(reader.GetOrdinal("user_id"));
                noteTagDatabase.Header = reader.GetString(reader.GetOrdinal("header"));
                noteTagDatabase.CreationDate = reader.GetDateTime(reader.GetOrdinal("creation_date"));
                noteTagDatabase.LastEditDate = reader.GetDateTime(reader.GetOrdinal("last_edit_date"));

                var sourcePath = reader.GetValue(reader.GetOrdinal("source_path"));

                if (sourcePath != DBNull.Value)
                    noteTagDatabase.SourcePath = sourcePath.ToString();
                
                noteTagDatabases.Add(noteTagDatabase);
            }

            return noteTagDatabases;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await connection.CloseAsync();
        }
    }
}