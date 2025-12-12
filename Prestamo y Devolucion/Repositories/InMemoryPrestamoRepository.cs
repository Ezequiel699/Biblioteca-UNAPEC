using System.Collections.Concurrent;
using BibliotecaAPEC.Models;
using BibliotecaAPEC.Utils;
namespace BibliotecaAPEC.Repositories;
public class InMemoryPrestamoRepository : IPrestamoRepository
{
    private readonly ConcurrentDictionary<int, Prestamo> _store = new();
    public Task<Prestamo?> GetByIdAsync(int id) => Task.FromResult(_store.TryGetValue(id, out var p) ? p : null);

    public IQueryable<Prestamo> QueryAll() => _store.Values.AsQueryable();

    public Task<Prestamo> AddAsync(Prestamo p)
    {
        var id = IdGenerator.NextId();
        p.Id = id;
        _store[id] = p;
        return Task.FromResult(p);
    }

    public Task UpdateAsync(Prestamo p)
    {
        _store[p.Id] = p;
        return Task.CompletedTask;
    }
}
