using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unapec.Biblioteca.Core.Entities;
using Unapec.Biblioteca.Infrastructure.Data;

namespace Unapec.Biblioteca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "amin,empleado")]

public class EditorasController : ControllerBase
{
    private readonly BibliotecaDbContext _context;

    public EditorasController(BibliotecaDbContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetEditoras")]
    public async Task<ActionResult<object>> GetEditoras([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? q = null, [FromQuery] bool? estado = null)
    {
        var query = _context.Editoras.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(e => e.Descripcion.Contains(q));
        if (estado.HasValue)
            query = query.Where(e => e.Estado == estado);

        var total = query.Count();
        var items = await query.OrderBy(e => e.Descripcion)
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

    [HttpGet("{id:int}", Name = "GetEditoraById")]
    public async Task<ActionResult<Editora>> GetEditora(int id)
    {
        var editora = await _context.Editoras.FindAsync(id);
        if (editora is null) return NotFound();
        return Ok(editora);
    }

    [HttpPost(Name = "CreateEditora")]
    public async Task<ActionResult<Editora>> CreateEditora(CreateEditoraRequest request)
    {
        var entity = new Editora
        {
            Descripcion = request.Descripcion,
            Estado = request.Estado
        };

        _context.Editoras.Add(entity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEditora), new { id = entity.Id }, entity);
    }

    [HttpPut("{id:int}", Name = "UpdateEditora")]
    public async Task<IActionResult> UpdateEditora(int id, UpdateEditoraRequest request)
    {
        var entity = await _context.Editoras.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Descripcion = request.Descripcion;
        entity.Estado = request.Estado;
        entity.ActualizadoEn = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteEditora")]
    public async Task<IActionResult> DeleteEditora(int id)
    {
        var entity = await _context.Editoras.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Estado = false;
        entity.ActualizadoEn = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

// Records definidos fuera de la clase del controlador (o en otro archivo)
public record CreateEditoraRequest(string Descripcion, bool Estado = true);
public record UpdateEditoraRequest(string Descripcion, bool Estado);