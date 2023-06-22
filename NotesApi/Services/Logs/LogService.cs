using System.Runtime.CompilerServices;
using Database.Logs;
using NotesApi.RefreshCookieAuthScheme;
using NotesApi.Services.Interfaces.Logs;
using Repositories.Interfaces.Logs;
using Repositories.Repositories.Logs;

namespace NotesApi.Services.Logs;

public class LogService<T> : ILogService
{
    private readonly ILogRepository _logRepository;

    private string CallerName => nameof(T);

    public LogService(RefreshCookieOptions options)
    {
        _logRepository = new LogsRepository(options.ConnectionString);
    }

    public LogService(IConfiguration configuration)
    {
        _logRepository = new LogsRepository(configuration);
    }

    public async Task Log(Guid userId, [CallerMemberName] string? callerMethod = null)
    {
        string log = CreateLogString(userId, null, callerMethod);
        
        var logDb = new LogsDatabase()
        {
            Action = log,
            NoteId = null,
            UserId = userId,
            TimeStamp = DateTime.UtcNow
        };

        await _logRepository.Create(logDb);
        
        Console.WriteLine(log);
    }

    public async Task Log(Guid userId, Guid noteId, [CallerMemberName] string? callerMethod = null)
    {
        string log = CreateLogString(userId, noteId, callerMethod);
        
        var logDb = new LogsDatabase()
        {
            Action = log,
            NoteId = null,
            UserId = userId,
            TimeStamp = DateTime.UtcNow
        };

        await _logRepository.Create(logDb);
        
        Console.WriteLine(log);
    }

    public async Task Log(Guid userId, string action, [CallerMemberName] string? callerMethod = null)
    {
        string log = CreateLogString(userId, action, null, callerMethod);
        
        var logDb = new LogsDatabase()
        {
            Action = log,
            NoteId = null,
            UserId = userId,
            TimeStamp = DateTime.UtcNow
        };

        await _logRepository.Create(logDb);
        
        Console.WriteLine(log);
    }

    public async Task Log(Guid userId, string action, Guid noteId, [CallerMemberName] string? callerMethod = null)
    {
        string log = CreateLogString(userId, action, noteId, callerMethod);
        
        var logDb = new LogsDatabase()
        {
            Action = log,
            NoteId = null,
            UserId = userId,
            TimeStamp = DateTime.UtcNow
        };

        await _logRepository.Create(logDb);
        
        Console.WriteLine(log);
    }

    public async Task<List<LogsDatabase>> GetUser(Guid userId)
    {
        return await _logRepository.GetUser(userId);
    }

    public async Task<List<LogsDatabase>> GetNote(Guid noteId)
    {
        return await _logRepository.GetNote(noteId);
    }

    public async Task DeleteUser(Guid userId)
    {
        await _logRepository.DeleteUser(userId);
    }

    public async Task DeleteNote(Guid noteId)
    {
        await _logRepository.DeleteNotes(noteId);
    }

    private string CreateLogString(Guid userId, string comment, Guid? noteId, string? callerMethod)
    {
        int blockLength = 30;

        string res = string.Empty;

        res += $"Caller: {CallerName}".PadRight(blockLength);

        res += $"UserId: {userId}".PadRight(blockLength);

        if (callerMethod != null)
            res += $"Action: {callerMethod}".PadRight(blockLength);

        res += $"Comment: {comment}".PadRight(blockLength);

        if (noteId != null)
            res += $"NoteId: {noteId}".PadRight(blockLength);

        return res;
    }

    private string CreateLogString(Guid userId, Guid? noteId, string? callerMethod)
    {
        int blockLength = 30;

        string res = string.Empty;

        res += $"Caller: {CallerName}".PadRight(blockLength);

        res += $"UserId: {userId}".PadRight(blockLength);

        if (callerMethod != null)
            res += $"Action: {callerMethod}".PadRight(blockLength);

        if (noteId != null)
            res += $"NoteId: {noteId}".PadRight(blockLength);

        return res;
    }
}