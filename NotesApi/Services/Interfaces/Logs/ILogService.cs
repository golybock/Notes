using System.Runtime.CompilerServices;
using Database.Logs;

namespace NotesApi.Services.Interfaces.Logs;

public interface ILogService
{
    public Task Log(Guid userId, [CallerMemberName] string? callerMethod = null);
    
    public Task Log(Guid userId, Guid noteId, [CallerMemberName] string? callerMethod = null);
    
    public Task Log(Guid userId, string action, [CallerMemberName] string? callerMethod = null);
    
    public Task Log(Guid userId, string action, Guid noteId, [CallerMemberName] string? callerMethod = null);

    public Task<List<LogsDatabase>> GetUser(Guid userId);

    public Task<List<LogsDatabase>> GetNote(Guid noteId);
    
    public Task DeleteUser(Guid userId);
    
    public Task DeleteNote(Guid noteId);
}