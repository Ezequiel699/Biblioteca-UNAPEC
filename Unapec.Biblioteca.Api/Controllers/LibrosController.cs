using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unapec.Biblioteca.Core.DTOs;
using Unapec.Biblioteca.Core.Entities;
using Unapec.Biblioteca.Infrastructure.Data;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

namespace Unapec.Biblioteca.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles ="amin,empleado")]
public class LibrosController : ControllerBase
{
    private readonly BibliotecaDbContext _context;
    private readonly IValidator<LibroCreateDto> _createValidator;
    private readonly IValidator<LibroUpdateDto> _updateValidator;

    public LibrosController(BibliotecaDbContext context,
                            IValidator<LibroCreateDto> createValidator,
                            IValidator<LibroUpdateDto> updateValidator)
    {
        _context = context;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet(Name = "GetLibros")]
    public async Task<ActionResult<object>> GetLibros([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? q = null, [FromQuery] bool? estado = null)
    {
        var query = _context.Set<Libro>()
            .Include(l => l.TipoBibliografia)
            .Include(l => l.Editora)
            .Include(l => l.Ciencia)
            .Include(l => l.Idioma)
            .Include(l => l.Autores)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(l => l.Descripcion.Contains(q) || (l.ISBN != null && l.ISBN.Contains(q)));

        if (estado.HasValue)
            query = query.Where(l => l.Estado == estado);

        var total = query.Count();
        var items = await query.OrderBy(l => l.Descripcion)
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

    [HttpGet("{id:int}", Name = "GetLibroById")]
    public async Task<ActionResult<Libro>> GetLibro(int id)
    {
        var libro = await _context.Set<Libro>()
            .Include(l => l.TipoBibliografia)
            .Include(l => l.Editora)
            .Include(l => l.Ciencia)
            .Include(l => l.Idioma)
            .Include(l => l.Autores)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (libro is null)
        {
            return NotFound();
        }

        return Ok(libro);
    }

    [HttpPost(Name = "CreateLibro")]
    public async Task<ActionResult<Libro>> CreateLibro(LibroCreateDto dto)
    {
        var vr = await _createValidator.ValidateAsync(dto);
        if (!vr.IsValid) return ValidationProblem(new ValidationProblemDetails(vr.ToDictionary()));

        // validar FKs
        async Task<bool> Exists<T>(DbSet<T> set, int id) where T : BaseCatalog => await set.AnyAsync(x => x.Id == id && x.Estado);
        if (!await Exists(_context.TiposBibliografia, dto.TipoBibliografiaId)) return BadRequest(new { error = "TipoBibliografiaId inválido" });
        if (!await Exists(_context.Editoras, dto.EditoraId)) return BadRequest(new { error = "EditoraId inválido" });
        if (!await Exists(_context.Ciencias, dto.CienciaId)) return BadRequest(new { error = "CienciaId inválido" });
        if (!await Exists(_context.Idiomas, dto.IdiomaId)) return BadRequest(new { error = "IdiomaId inválido" });

        var autores = (dto.AutorIds is { Count: > 0 })
            ? await _context.Autores.Where(a => dto.AutorIds!.Contains(a.Id) && a.Estado).ToListAsync()
            : new List<Autor>();

        if (dto.AutorIds is { Count: > 0 } && autores.Count != dto.AutorIds.Count)
            return BadRequest(new { error = "Uno o más AutorIds no existen o están inactivos" });

        var entity = new Libro
        {
            Descripcion = dto.Descripcion,
            SignaturaTopografica = dto.SignaturaTopografica,
            ISBN = dto.ISBN,
            TipoBibliografiaId = dto.TipoBibliografiaId,
            EditoraId = dto.EditoraId,
            AnioPublicacion = dto.AnioPublicacion,
            CienciaId = dto.CienciaId,
            IdiomaId = dto.IdiomaId,
            Estado = dto.Estado,
            Autores = autores
        };

        _context.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetLibro), new { id = entity.Id }, entity);
    }

    [HttpPut("{id:int}", Name = "UpdateLibro")]
    public async Task<IActionResult> UpdateLibro(int id, LibroUpdateDto dto)
    {
        var vr = await _updateValidator.ValidateAsync(dto);
        if (!vr.IsValid) return ValidationProblem(new ValidationProblemDetails(vr.ToDictionary()));

        var entity = await _context.Set<Libro>()
            .Include(l => l.Autores)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (entity is null) return NotFound();

        // validar FKs
        async Task<bool> Exists<T>(DbSet<T> set, int fk) where T : BaseCatalog => await set.AnyAsync(x => x.Id == fk && x.Estado);
        if (!await Exists(_context.TiposBibliografia, dto.TipoBibliografiaId)) return BadRequest(new { error = "TipoBibliografiaId inválido" });
        if (!await Exists(_context.Editoras, dto.EditoraId)) return BadRequest(new { error = "EditoraId inválido" });
        if (!await Exists(_context.Ciencias, dto.CienciaId)) return BadRequest(new { error = "CienciaId inválido" });
        if (!await Exists(_context.Idiomas, dto.IdiomaId)) return BadRequest(new { error = "IdiomaId inválido" });

        entity.Descripcion = dto.Descripcion;
        entity.SignaturaTopografica = dto.SignaturaTopografica;
        entity.ISBN = dto.ISBN;
        entity.TipoBibliografiaId = dto.TipoBibliografiaId;
        entity.EditoraId = dto.EditoraId;
        entity.AnioPublicacion = dto.AnioPublicacion;
        entity.CienciaId = dto.CienciaId;
        entity.IdiomaId = dto.IdiomaId;
        entity.Estado = dto.Estado;
        entity.ActualizadoEn = DateTime.UtcNow;

        // actualizar autores (many-to-many)
        entity.Autores.Clear();
        if (dto.AutorIds is { Count: > 0 })
        {
            var autores = await _context.Autores.Where(a => dto.AutorIds.Contains(a.Id) && a.Estado).ToListAsync();
            if (autores.Count != dto.AutorIds.Count)
                return BadRequest(new { error = "Uno o más AutorIds no existen o están inactivos" });
            foreach (var a in autores) entity.Autores.Add(a);
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Soft delete
    [HttpDelete("{id:int}", Name = "DeleteLibro")]
    public async Task<IActionResult> DeleteLibro(int id)
    {
        var entity = await _context.Set<Libro>().FindAsync(id);
        if (entity is null) return NotFound();
        entity.Estado = false;
        entity.ActualizadoEn = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Método auxiliar privado para validación de FKs
    private async Task<bool> Exists<T>(DbSet<T> set, int id) where T : BaseCatalog
    {
        return await set.AnyAsync(x => x.Id == id && x.Estado);
    }
}