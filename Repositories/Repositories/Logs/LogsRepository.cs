using Database.Logs;
using Microsoft.Extensions.Configuration;
using Repositories.Interfaces.Logs;

namespace Repositories.Repositories.Logs;

public class LogsRepository : RepositoryBase, ILogRepository
{
    public LogsRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public LogsRepository(string connectionString) : base(connectionString)
    {
    }

    public async Task<LogsDatabase?> Get(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<LogsDatabase>> GetUser(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<LogsDatabase>> GetNote(Guid noteId)
    {
        throw new NotImplementedException();
    }

    public async Task<int> Create(LogsDatabase noteDatabase)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteUser(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteNotes(Guid noteId)
    {
        throw new NotImplementedException();
    }
}