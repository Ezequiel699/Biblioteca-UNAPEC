using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unapec.Biblioteca.Core.Entities;
using Unapec.Biblioteca.Infrastructure.Data;

namespace Unapec.Biblioteca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrestamosController : ControllerBase
{
    private readonly BibliotecaDbContext _context;

    public PrestamosController(BibliotecaDbContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetPrestamos")]
    public async Task<ActionResult<object>> GetPrestamos(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? usuarioId = null,
        [FromQuery] int? libroId = null,
        [FromQuery] bool? devuelto = null)
    {
        var query = _context.Prestamos
            .Include(p => p.Usuario)
            .Include(p => p.Libro)
            .AsQueryable();

        if (usuarioId.HasValue)
            query = query.Where(p => p.UsuarioId == usuarioId.Value);
        if (libroId.HasValue)
            query = query.Where(p => p.LibroId == libroId.Value);
        if (devuelto.HasValue)
            query = query.Where(p => p.Devuelto == devuelto.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(p => p.FechaPrestamo)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new
        {
            page,
            pageSize,
            total,
            items
        });
    }

    [HttpGet("{id:int}", Name = "GetPrestamoById")]
    public async Task<ActionResult<Prestamo>> GetPrestamo(int id)
    {
        var prestamo = await _context.Prestamos
            .Include(p => p.Usuario)
            .Include(p => p.Libro)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (prestamo == null)
            return NotFound();

        return Ok(prestamo);
    }

    [HttpPost(Name = "CreatePrestamo")]
    public async Task<ActionResult<Prestamo>> CreatePrestamo(Prestamo request)
    {
        // Validar solo los IDs (ignorar cualquier objeto anidado que venga)
        if (request.UsuarioId <= 0 || request.LibroId <= 0)
            return BadRequest("UsuarioId y LibroId son requeridos.");

        // Verificar existencia y estado del libro
        var libro = await _context.Libros.FindAsync(request.LibroId);
        if (libro == null)
            return BadRequest("El libro no existe.");

        // Verificar existencia del usuario
        var usuario = await _context.Usuarios.FindAsync(request.UsuarioId);
        if (usuario == null)
            return BadRequest("El usuario no existe.");

        // Verificar que no esté ya prestado
        var activo = await _context.Prestamos.AnyAsync(p => p.LibroId == request.LibroId && !p.Devuelto);
        if (activo)
            return BadRequest("El libro ya está prestado y no ha sido devuelto.");

        // ✅ Crear una NUEVA instancia limpia de Prestamo (¡sin usar el objeto entrante directamente!)
        var prestamo = new Prestamo
        {
            UsuarioId = request.UsuarioId,
            LibroId = request.LibroId,
            FechaPrestamo = DateTime.UtcNow,
            Devuelto = false,
            CreadoEn = DateTime.UtcNow
        };

        _context.Prestamos.Add(prestamo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPrestamo), new { id = prestamo.Id }, prestamo);
    }
    [HttpPut("devolver/{id:int}", Name = "DevolverPrestamo")]
    public async Task<IActionResult> DevolverPrestamo(int id)
    {
        var prestamo = await _context.Prestamos.FindAsync(id);
        if (prestamo == null)
            return NotFound();

        if (prestamo.Devuelto)
            return BadRequest("El préstamo ya fue devuelto.");

        prestamo.Devuelto = true;
        prestamo.FechaDevolucion = DateTime.UtcNow;
        prestamo.ActualizadoEn = DateTime.UtcNow;

        _context.Entry(prestamo).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // ⚠️ Opcional: ¿Quieres borrar físicamente un préstamo?
    // En sistemas de biblioteca, rara vez se borran préstamos.
    // Si decides no borrarlos, puedes omitir este método o dejarlo deshabilitado.
    /*
    [HttpDelete("{id:int}", Name = "DeletePrestamo")]
    public async Task<IActionResult> DeletePrestamo(int id)
    {
        var prestamo = await _context.Prestamos.FindAsync(id);
        if (prestamo == null)
            return NotFound();

        _context.Prestamos.Remove(prestamo);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    */
}