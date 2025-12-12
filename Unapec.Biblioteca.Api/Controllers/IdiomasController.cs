using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unapec.Biblioteca.Core.Entities;
using Unapec.Biblioteca.Infrastructure.Data;

namespace Unapec.Biblioteca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles = "amin,empleado")]
public class IdiomasController : ControllerBase
{
    private readonly BibliotecaDbContext _context;

    public IdiomasController(BibliotecaDbContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetIdiomas")]
    public async Task<ActionResult<object>> GetIdiomas([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? q = null, [FromQuery] bool? estado = null)
    {
        var query = _context.Idiomas.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(i => i.Descripcion.Contains(q));
        if (estado.HasValue)
            query = query.Where(i => i.Estado == estado);

        var total = query.Count();
        var items = await query.OrderBy(i => i.Descripcion)
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

    [HttpGet("{id:int}", Name = "GetIdiomaById")]
    public async Task<ActionResult<Idioma>> GetIdioma(int id)
    {
        var idioma = await _context.Idiomas.FindAsync(id);
        if (idioma is null) return NotFound();
        return Ok(idioma);
    }

    [HttpPost(Name = "CreateIdioma")]
    public async Task<ActionResult<Idioma>> CreateIdioma(CreateIdiomaRequest request)
    {
        var entity = new Idioma
        {
            Descripcion = request.Descripcion,
            Estado = request.Estado
        };

        _context.Idiomas.Add(entity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetIdioma), new { id = entity.Id }, entity);
    }

    [HttpPut("{id:int}", Name = "UpdateIdioma")]
    public async Task<IActionResult> UpdateIdioma(int id, UpdateIdiomaRequest request)
    {
        var entity = await _context.Idiomas.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Descripcion = request.Descripcion;
        entity.Estado = request.Estado;
        entity.ActualizadoEn = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteIdioma")]
    public async Task<IActionResult> DeleteIdioma(int id)
    {
        var entity = await _context.Idiomas.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Estado = false;
        entity.ActualizadoEn = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

// Records definidos fuera de la clase del controlador (o en otro archivo)
public record CreateIdiomaRequest(string Descripcion, bool Estado = true);
public record UpdateIdiomaRequest(string Descripcion, bool Estado);