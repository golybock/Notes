using Database.Note;

namespace Repositories.Repositories.Interfaces.Note;

public interface IPermissionsLevelRepository
{
    public Task<PermissionsLevelDatabase?> Get(int id);
    
    public Task<List<PermissionsLevelDatabase>> Get();
}