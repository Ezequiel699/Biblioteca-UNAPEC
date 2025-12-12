using System.Collections.Concurrent;
using BibliotecaAPEC.Utils;
namespace BibliotecaAPEC.Repositories;
public class InMemoryGenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ConcurrentDictionary<int, T> _store = new();
    protected readonly Func<T, int> _getId;
    protected readonly Action<T, int> _setId;

    public InMemoryGenericRepository(Func<T,int> getId, Action<T,int> setId)
    {
        _getId = getId;
        _setId = setId;
    }

    public Task<T?> GetByIdAsync(int id) => Task.FromResult(_store.TryGetValue(id, out var v) ? v : null);

    public Task<IEnumerable<T>> GetAllAsync() => Task.FromResult<IEnumerable<T>>(_store.Values.ToList());

    public Task<T> AddAsync(T entity)
    {
        var id = IdGenerator.NextId();
        _setId(entity, id);
        _store[id] = entity;
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(T entity)
    {
        var id = _getId(entity);
        _store[id] = entity;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(int id)
    {
        _store.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}
