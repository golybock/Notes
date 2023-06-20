using Database.Logs;
using Npgsql;

namespace Repositories.Readers.Logs;

public class LogsReader : IReader<LogsDatabase>
{
    private const string Id = "id";
    private const string UserId = "user_id";
    private const string NoteId = "note_id";
    private const string Action = "action";
    private const string Timestamp = "timestamp";

    public static async Task<LogsDatabase?> ReadAsync(NpgsqlDataReader reader)
    {
        while (await reader.ReadAsync())
        {
            LogsDatabase logsDatabase = new LogsDatabase();
            
            logsDatabase.Id = reader.GetInt32(reader.GetOrdinal(Id));
            logsDatabase.UserId = reader.GetGuid(reader.GetOrdinal(UserId));
            logsDatabase.Action = reader.GetString(reader.GetOrdinal(Action));
            logsDatabase.TimeStamp = reader.GetDateTime(reader.GetOrdinal(Timestamp));

            var noteId = reader.GetValue(reader.GetOrdinal(NoteId));

            logsDatabase.NoteId = noteId == DBNull.Value ? null : (Guid) noteId;

            return logsDatabase;
        }

        return null;
    }

    public static async Task<List<LogsDatabase>> ReadListAsync(NpgsqlDataReader reader)
    {
        List<LogsDatabase> logsDatabases = new List<LogsDatabase>();

        while (await reader.ReadAsync())
        {
            LogsDatabase logsDatabase = new LogsDatabase();
            
            logsDatabase.Id = reader.GetInt32(reader.GetOrdinal(Id));
            logsDatabase.UserId = reader.GetGuid(reader.GetOrdinal(UserId));
            logsDatabase.Action = reader.GetString(reader.GetOrdinal(Action));
            logsDatabase.TimeStamp = reader.GetDateTime(reader.GetOrdinal(Timestamp));

            var noteId = reader.GetValue(reader.GetOrdinal(NoteId));

            logsDatabase.NoteId = noteId == DBNull.Value ? null : (Guid) noteId;
            
            logsDatabases.Add(logsDatabase);
        }

        return logsDatabases;
    }
}