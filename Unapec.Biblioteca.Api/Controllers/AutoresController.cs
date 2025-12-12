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
[Authorize(Roles = "amin,empleado")]

public class AutoresController : ControllerBase
{
    private readonly BibliotecaDbContext _context;
    private readonly IValidator<AutorCreateDto> _createValidator;
    private readonly IValidator<AutorUpdateDto> _updateValidator;

    public AutoresController(BibliotecaDbContext context,
                             IValidator<AutorCreateDto> createValidator,
                             IValidator<AutorUpdateDto> updateValidator)
    {
        _context = context;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet(Name = "GetAutores")]
    public async Task<ActionResult<object>> GetAutores([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? q = null, [FromQuery] int? idiomaId = null, [FromQuery] bool? estado = null)
    {
        var query = _context.Autores
            .Include(a => a.IdiomaNativo)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(a => a.Nombre.Contains(q));
        if (idiomaId.HasValue) query = query.Where(a => a.IdiomaNativoId == idiomaId);
        if (estado.HasValue) query = query.Where(a => a.Estado == estado);

        var total = query.Count();
        var items = await query.OrderBy(a => a.Nombre)
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

    [HttpGet("{id:int}", Name = "GetAutorById")]
    public async Task<ActionResult<Autor>> GetAutor(int id)
    {
        var autor = await _context.Autores.Include(a => a.IdiomaNativo).FirstOrDefaultAsync(a => a.Id == id);

        if (autor is null)
        {
            return NotFound();
        }

        return Ok(autor);
    }

    [HttpPost(Name = "CreateAutor")]
    public async Task<ActionResult<Autor>> CreateAutor(AutorCreateDto dto)
    {
        var vr = await _createValidator.ValidateAsync(dto);
        if (!vr.IsValid) return ValidationProblem(new ValidationProblemDetails(vr.ToDictionary()));

        // verificar FK idioma
        var idiomaExists = await _context.Idiomas.AnyAsync(i => i.Id == dto.IdiomaNativoId && i.Estado);
        if (!idiomaExists) return BadRequest(new { error = "IdiomaNativoId inválido" });

        var entity = new Autor
        {
            Nombre = dto.Nombre,
            PaisOrigen = dto.PaisOrigen,
            IdiomaNativoId = dto.IdiomaNativoId,
            Estado = dto.Estado
        };

        _context.Autores.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAutor), new { id = entity.Id }, entity);
    }

    [HttpPut("{id:int}", Name = "UpdateAutor")]
    public async Task<IActionResult> UpdateAutor(int id, AutorUpdateDto dto)
    {
        var vr = await _updateValidator.ValidateAsync(dto);
        if (!vr.IsValid) return ValidationProblem(new ValidationProblemDetails(vr.ToDictionary()));

        var entity = await _context.Autores.FindAsync(id);
        if (entity is null) return NotFound();

        if (!await _context.Idiomas.AnyAsync(i => i.Id == dto.IdiomaNativoId && i.Estado))
            return BadRequest(new { error = "IdiomaNativoId inválido" });

        entity.Nombre = dto.Nombre;
        entity.PaisOrigen = dto.PaisOrigen;
        entity.IdiomaNativoId = dto.IdiomaNativoId;
        entity.Estado = dto.Estado;
        entity.ActualizadoEn = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Soft delete
    [HttpDelete("{id:int}", Name = "DeleteAutor")]
    public async Task<IActionResult> DeleteAutor(int id)
    {
        var entity = await _context.Autores.FindAsync(id);
        if (entity is null) return NotFound();
        entity.Estado = false;
        entity.ActualizadoEn = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}