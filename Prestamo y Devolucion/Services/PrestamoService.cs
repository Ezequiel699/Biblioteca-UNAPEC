using BibliotecaAPEC.Models;
using BibliotecaAPEC.Repositories;
using BibliotecaAPEC.Utils;
namespace BibliotecaAPEC.Services;
public class PrestamoService : IPrestamoService
{
    private readonly IPrestamoRepository _prestRepo;
    private readonly IGenericRepository<Book> _libRepo;
    private readonly IGenericRepository<Usuario> _userRepo;

    public PrestamoService(IPrestamoRepository prestRepo,
                          IGenericRepository<Book> libRepo,
                          IGenericRepository<Usuario> userRepo)
    {
        _prestRepo = prestRepo;
        _libRepo = libRepo;
        _userRepo = userRepo;
    }

    public async Task<(bool ok, string message, Prestamo? prestamo)> PrestarAsync(int usuarioId, int libroId)
    {
        var usuario = await _userRepo.GetByIdAsync(usuarioId);
        if (usuario == null) return (false, "Usuario no encontrado", null);

        var libro = await _libRepo.GetByIdAsync(libroId);
        if (libro == null) return (false, "Libro no encontrado", null);
        if (!libro.Disponible) return (false, "Libro no disponible", null);

        // crear prestamo
        var prestamo = new Prestamo
        {
            UsuarioId = usuarioId,
            Usuario = usuario,
            LibroId = libroId,
            Libro = libro,
            FechaPrestamo = DateTime.UtcNow
        };

        libro.Disponible = false;
        await _libRepo.UpdateAsync(libro);

        var created = await _prestRepo.AddAsync(prestamo);
        return (true, "Prestamo creado", created);
    }

    public async Task<(bool ok, string message)> DevolverAsync(int prestamoId)
    {
        var prestamo = await _prestRepo.GetByIdAsync(prestamoId);
        if (prestamo == null) return (false, "Prestamo no encontrado");

        if (prestamo.Devuelto) return (false, "Ya fue devuelto");

        prestamo.FechaDevolucion = DateTime.UtcNow;
        await _prestRepo.UpdateAsync(prestamo);

        // marcar libro disponible (buscar en repo de libros)
        var libro = await _libRepo.GetByIdAsync(prestamo.LibroId);
        if (libro != null)
        {
            libro.Disponible = true;
            await _libRepo.UpdateAsync(libro);
        }

        return (true, "Libro devuelto correctamente");
    }

    public async Task<PagedResult<Prestamo>> BuscarAsync(PrestamoQueryParams qp)
    {
        var q = _prestRepo.QueryAll();

        if (qp.UsuarioId.HasValue) q = q.Where(x => x.UsuarioId == qp.UsuarioId.Value);
        if (!string.IsNullOrEmpty(qp.Categoria)) q = q.Where(x => x.Libro.Categoria == qp.Categoria);
        if (!string.IsNullOrEmpty(qp.Idioma)) q = q.Where(x => x.Libro.Idioma == qp.Idioma);
        if (qp.Desde.HasValue) q = q.Where(x => x.FechaPrestamo >= qp.Desde.Value);
        if (qp.Hasta.HasValue) q = q.Where(x => x.FechaPrestamo <= qp.Hasta.Value);

        var total = q.Count();
        var items = q.Skip((qp.Page - 1) * qp.PageSize).Take(qp.PageSize).ToList();

        return new PagedResult<Prestamo>
        {
            Items = items,
            Total = total,
            Page = qp.Page,
            PageSize = qp.PageSize
        };
    }

    public Task<Prestamo?> GetByIdAsync(int id) => _prestRepo.GetByIdAsync(id);
}
