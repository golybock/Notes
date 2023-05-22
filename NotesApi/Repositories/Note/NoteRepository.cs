using Database.Note;
using NotesApi.Repositories.Interfaces.Note;
using Npgsql;

namespace NotesApi.Repositories.Note;

public class NoteRepository : RepositoryBase, INoteRepository
{
    public NoteRepository(IConfiguration configuration) : base(configuration)
    {
    }

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
                Parameters = { new NpgsqlParameter() { Value = id } }
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

    public async Task<int> Create(NoteDatabase noteDatabase)
    {
        string query = "insert into note(header, creation_date, last_edit_date, source_path, user_id)" +
                       "values ($1, $2, $3, $4, $5) returning id";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = noteDatabase.Header },
                    new NpgsqlParameter() { Value = noteDatabase.CreationDate },
                    new NpgsqlParameter() { Value = noteDatabase.LastEditDate },
                    new NpgsqlParameter() { Value = noteDatabase.SourcePath},
                    new NpgsqlParameter() { Value = noteDatabase.UserId}
                }
            };

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                return reader.GetInt32(reader.GetOrdinal("id"));

            return 1;
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

    public async Task<int> Update(int id, NoteDatabase noteDatabase)
    {
        string query = "update note set header = $2, last_edit_date = $3," +
                       " source_path = $4 where id = $1";

        var connection = GetConnection();

        try
        {
            await connection.OpenAsync();

            NpgsqlCommand command = new NpgsqlCommand(query, connection)
            {
                Parameters =
                {
                    new NpgsqlParameter() { Value = id},
                    new NpgsqlParameter() { Value = noteDatabase.Header },
                    new NpgsqlParameter() { Value = noteDatabase.LastEditDate },
                    new NpgsqlParameter() { Value = noteDatabase.SourcePath},
                }
            };

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
                return reader.GetInt32(reader.GetOrdinal("id"));

            return 1;
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

    public async Task<int> Delete(int id)
    {
        return await DeleteAsync("note", "id", id);
    }
}