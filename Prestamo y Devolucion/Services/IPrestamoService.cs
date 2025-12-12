using BibliotecaAPEC.Models;
using BibliotecaAPEC.Utils;
namespace BibliotecaAPEC.Services;
public interface IPrestamoService
{
    Task<(bool ok, string message, Prestamo? prestamo)> PrestarAsync(int usuarioId, int libroId);
    Task<(bool ok, string message)> DevolverAsync(int prestamoId);
    Task<PagedResult<Prestamo>> BuscarAsync(PrestamoQueryParams qp);
    Task<Prestamo?> GetByIdAsync(int id);
}
