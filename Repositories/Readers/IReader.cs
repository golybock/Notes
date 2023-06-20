using Npgsql;

namespace Repositories.Readers;

public interface IReader<T>
{
    public static abstract Task<T?> ReadAsync(NpgsqlDataReader reader);
    
    public static abstract Task<List<T>> ReadListAsync(NpgsqlDataReader reader);
}