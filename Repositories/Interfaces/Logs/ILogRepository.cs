using Database.Logs;

namespace Repositories.Interfaces.Logs;

public interface ILogRepository
{
    public Task<LogsDatabase?> Get(int id);

    public Task<List<LogsDatabase>> GetUser(Guid userId);
    
    public Task<List<LogsDatabase>> GetNote(Guid noteId);

    public Task<int> Create(LogsDatabase noteDatabase);

    public Task<int> Delete(int id);

    public Task<int> DeleteUser(Guid userId);
    
    public Task<int> DeleteNotes(Guid noteId);
}