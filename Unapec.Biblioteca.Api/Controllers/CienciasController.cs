using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unapec.Biblioteca.Core.Entities;
using Unapec.Biblioteca.Infrastructure.Data;

namespace Unapec.Biblioteca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "amin,empleado")]

public class CienciasController : ControllerBase
{
    private readonly BibliotecaDbContext _context;

    public CienciasController(BibliotecaDbContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetCiencias")]
    public async Task<ActionResult<object>> GetCiencias([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? q = null, [FromQuery] bool? estado = null)
    {
        var query = _context.Ciencias.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(c => c.Descripcion.Contains(q));
        if (estado.HasValue)
            query = query.Where(c => c.Estado == estado);

        var total = query.Count();
        var items = await query.OrderBy(c => c.Descripcion)
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

    [HttpGet("{id:int}", Name = "GetCienciaById")]
    public async Task<ActionResult<Ciencia>> GetCiencia(int id)
    {
        var ciencia = await _context.Ciencias.FindAsync(id);
        if (ciencia is null) return NotFound();
        return Ok(ciencia);
    }

    [HttpPost(Name = "CreateCiencia")]
    public async Task<ActionResult<Ciencia>> CreateCiencia(CreateCienciaRequest request)
    {
        var entity = new Ciencia
        {
            Descripcion = request.Descripcion,
            Estado = request.Estado
        };

        _context.Ciencias.Add(entity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCiencia), new { id = entity.Id }, entity);
    }

    [HttpPut("{id:int}", Name = "UpdateCiencia")]
    public async Task<IActionResult> UpdateCiencia(int id, UpdateCienciaRequest request)
    {
        var entity = await _context.Ciencias.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Descripcion = request.Descripcion;
        entity.Estado = request.Estado;
        entity.ActualizadoEn = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteCiencia")]
    public async Task<IActionResult> DeleteCiencia(int id)
    {
        var entity = await _context.Ciencias.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Estado = false;
        entity.ActualizadoEn = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

// Records definidos fuera de la clase del controlador (o en otro archivo)
public record CreateCienciaRequest(string Descripcion, bool Estado = true);
public record UpdateCienciaRequest(string Descripcion, bool Estado);