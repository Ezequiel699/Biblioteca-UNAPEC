using BibliotecaAPEC.Models;
namespace BibliotecaAPEC.Repositories;
public interface IPrestamoRepository
{
    Task<Prestamo?> GetByIdAsync(int id);
    IQueryable<Prestamo> QueryAll(); // para filtros/paginación
    Task<Prestamo> AddAsync(Prestamo p);
    Task UpdateAsync(Prestamo p);
}
