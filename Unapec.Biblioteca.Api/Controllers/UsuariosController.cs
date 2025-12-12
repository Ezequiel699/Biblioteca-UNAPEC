using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unapec.Biblioteca.Core.DTOs;
using Unapec.Biblioteca.Core.Entities;
using Unapec.Biblioteca.Infrastructure.Data;

namespace Unapec.Biblioteca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin,empleado")] // Corregido typo: "amin" -> "admin"
public class UsuariosController : ControllerBase
{
    private readonly BibliotecaDbContext _context;
    private readonly IValidator<UsuarioCreateDto> _createValidator;
    private readonly IValidator<UsuarioUpdateDto> _updateValidator;

    public UsuariosController(BibliotecaDbContext context,
                              IValidator<UsuarioCreateDto> createValidator,
                              IValidator<UsuarioUpdateDto> updateValidator)
    {
        _context = context;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet(Name = "GetUsuarios")]
    public async Task<ActionResult<List<Usuario>>> GetUsuarios([FromQuery] bool? estado = null)
    {
        var query = _context.Usuarios.AsQueryable();
        if (estado.HasValue) query = query.Where(u => u.Estado == estado);
        var items = await query.OrderBy(u => u.Nombre).ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}", Name = "GetUsuarioById")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario is null) return NotFound();
        return Ok(usuario);
    }

    [HttpPost(Name = "CreateUsuario")]
    public async Task<ActionResult<Usuario>> CreateUsuario(UsuarioCreateDto dto)
    {
        var vr = await _createValidator.ValidateAsync(dto);
        if (!vr.IsValid)
            return ValidationProblem(new ValidationProblemDetails(vr.ToDictionary()));

        var entity = new Usuario
        {
            Nombre = dto.Nombre,
            Cedula = dto.Cedula,
            NoCarnet = dto.NoCarnet,
            TipoPersona = dto.TipoPersona,
            Estado = dto.Estado
        };

        _context.Usuarios.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUsuario), new { id = entity.Id }, entity);
    }

    [HttpPut("{id:int}", Name = "UpdateUsuario")]
    public async Task<IActionResult> UpdateUsuario(int id, UsuarioUpdateDto dto)
    {
        var vr = await _updateValidator.ValidateAsync(dto);
        if (!vr.IsValid)
            return ValidationProblem(new ValidationProblemDetails(vr.ToDictionary()));

        var entity = await _context.Usuarios.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Nombre = dto.Nombre;
        entity.Cedula = dto.Cedula;
        entity.NoCarnet = dto.NoCarnet;
        entity.TipoPersona = dto.TipoPersona;
        entity.Estado = dto.Estado;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Soft delete
    [HttpDelete("{id:int}", Name = "DeleteUsuario")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var entity = await _context.Usuarios.FindAsync(id);
        if (entity is null) return NotFound();
        entity.Estado = false;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // --- Nuevas Acciones para Préstamo/Devolución ---

    /// <summary>
    /// Obtiene la lista de libros actualmente prestados por un usuario específico.
    /// </summary>
    /// <param name="id">ID del usuario.</param>
    /// <returns>Lista de préstamos activos (no devueltos).</returns>
    [HttpGet("{id:int}/libros-prestados", Name = "GetLibrosPrestadosByUsuario")]
    public async Task<ActionResult<IEnumerable<Prestamo>>> GetLibrosPrestados(int id)
    {
        // Verificar si el usuario existe y está activo
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario is null || !usuario.Estado)
        {
            return NotFound(new { error = "Usuario no encontrado o inactivo." });
        }

        // Buscar préstamos activos (no devueltos) asociados al usuario
        var prestamosActivos = await _context.Prestamos
            .Where(p => p.UsuarioId == id && !p.Devuelto) // Asumiendo que 'Devuelto' indica si fue devuelto
            .Include(p => p.Libro) // Incluir datos del libro prestado
            .OrderByDescending(p => p.FechaPrestamo) // Ordenar por fecha, más reciente primero
            .ToListAsync();

        return Ok(prestamosActivos);
    }


    /// <summary>
    /// Registra un nuevo préstamo de un libro a un usuario.
    /// </summary>
    /// <param name="id">ID del usuario que toma prestado el libro.</param>
    /// <param name="request">Contiene el ID del libro a prestar.</param>
    /// <returns>Detalles del préstamo registrado.</returns>
    [HttpPost("{id:int}/tomar-prestado", Name = "TomarLibroPrestado")]
    public async Task<ActionResult<Prestamo>> TomarLibroPrestado(int id, [FromBody] PrestamoRequest request)
    {
        // Verificar si el usuario existe y está activo
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario is null || !usuario.Estado)
        {
            return NotFound(new { error = "Usuario no encontrado o inactivo." });
        }

        // Verificar si el libro existe, está activo y no está actualmente prestado
        var libro = await _context.Libros
            .Where(l => l.Id == request.LibroId && l.Estado)
            .FirstOrDefaultAsync();

        if (libro is null)
        {
            return BadRequest(new { error = "Libro no encontrado o inactivo." });
        }

        // Verificar si el libro ya está prestado (buscando un préstamo activo)
        var libroPrestado = await _context.Prestamos
            .AnyAsync(p => p.LibroId == request.LibroId && !p.Devuelto);

        if (libroPrestado)
        {
            return BadRequest(new { error = "El libro ya está prestado." });
        }

        // Crear el nuevo registro de préstamo
        var nuevoPrestamo = new Prestamo
        {
            UsuarioId = id,
            LibroId = request.LibroId,
            FechaPrestamo = DateTime.UtcNow, // Fecha actual
            Devuelto = false // No devuelto inicialmente
        };

        _context.Prestamos.Add(nuevoPrestamo);
        await _context.SaveChangesAsync();

        // Volver a cargar con las propiedades de navegación para la respuesta
        await _context.Entry(nuevoPrestamo)
            .Reference(p => p.Usuario)
            .LoadAsync();
        await _context.Entry(nuevoPrestamo)
            .Reference(p => p.Libro)
            .LoadAsync();

        return CreatedAtAction(nameof(GetLibrosPrestados), new { id = id }, nuevoPrestamo);
    }

    /// <summary>
    /// Registra la devolución de un libro previamente prestado.
    /// </summary>
    /// <param name="id">ID del usuario que devuelve el libro.</param>
    /// <param name="request">Contiene el ID del préstamo a cerrar.</param>
    /// <returns>No Content.</returns>
    [HttpPost("{id:int}/devolver-libro", Name = "DevolverLibroPrestado")]
    public async Task<IActionResult> DevolverLibroPrestado(int id, [FromBody] DevolucionRequest request)
    {
        // Verificar si el usuario existe y está activo
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario is null || !usuario.Estado)
        {
            return NotFound(new { error = "Usuario no encontrado o inactivo." });
        }

        // Buscar el préstamo específico que no haya sido devuelto aún
        var prestamo = await _context.Prestamos
            .Where(p => p.Id == request.PrestamoId && p.UsuarioId == id && !p.Devuelto)
            .FirstOrDefaultAsync();

        if (prestamo is null)
        {
            return NotFound(new { error = "Préstamo no encontrado, no pertenece al usuario o ya fue devuelto." });
        }

        // Marcar el préstamo como devuelto
        prestamo.Devuelto = true;
        prestamo.FechaDevolucion = DateTime.UtcNow;
        prestamo.ActualizadoEn = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}

// --- DTOs para las nuevas acciones ---
public record PrestamoRequest(int LibroId);
public record DevolucionRequest(int PrestamoId);