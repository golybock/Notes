using Database.Note.Tag;
using NotesApi.Repositories.Interfaces.Note.Tag;
using Npgsql;

namespace NotesApi.Repositories.Note.Tag;

public class NoteTagRepository : RepositoryBase, INoteTagRepository
{
    public NoteTagRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<NoteTagDatabase?> Get(int id)
    {
        NoteTagDatabase noteTagDatabase = new NoteTagDatabase();
        
        string query = "select * from note_tag where id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters = {new NpgsqlParameter() {Value = id}}
            };

            await using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                noteTagDatabase.Id = reader.GetInt32(reader.GetOrdinal("id"));
                noteTagDatabase.NoteId = reader.GetInt32(reader.GetOrdinal("note_id"));
                noteTagDatabase.TagId = reader.GetInt32(reader.GetOrdinal("tag_id"));
                
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
}