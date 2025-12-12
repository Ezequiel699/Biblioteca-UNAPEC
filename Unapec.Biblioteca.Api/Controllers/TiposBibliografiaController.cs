// TiposBibliografiaController.cs
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unapec.Biblioteca.Core.DTOs;
using Unapec.Biblioteca.Core.Entities;
using Unapec.Biblioteca.Infrastructure.Data;

namespace Unapec.Biblioteca.Controllers;

[ApiController]
[Route("api/tipos-bibliografia")]
[Produces("application/json")]
[Authorize(Roles = "amin,empleado")]
public class TiposBibliografiaController : ControllerBase
{
    private readonly BibliotecaDbContext _db;
    private readonly IValidator<CatalogCreateDto> _createValidator;
    private readonly IValidator<CatalogUpdateDto> _updateValidator;

    public TiposBibliografiaController(
        BibliotecaDbContext db,
        IValidator<CatalogCreateDto> createValidator,
        IValidator<CatalogUpdateDto> updateValidator)
    {
        _db = db;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? q = null,
        [FromQuery] bool? estado = null)
    {
        var query = _db.TiposBibliografia.AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(x => x.Descripcion.Contains(q));
        if (estado.HasValue)
            query = query.Where(x => x.Estado == estado);

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(x => x.Id)
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _db.TiposBibliografia.FindAsync(id);
        if (entity == null) return NotFound();
        return Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CatalogCreateDto dto)
    {
        var vr = await _createValidator.ValidateAsync(dto);
        if (!vr.IsValid)
            return ValidationProblem(new ValidationProblemDetails(vr.ToDictionary()));

        var entity = new TipoBibliografia
        {
            Descripcion = dto.Descripcion,
            Estado = dto.Estado
        };

        _db.TiposBibliografia.Add(entity);
        await _db.SaveChangesAsync();

        return Created($"/api/tipos-bibliografia/{entity.Id}", entity);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CatalogUpdateDto dto)
    {
        var vr = await _updateValidator.ValidateAsync(dto);
        if (!vr.IsValid)
            return ValidationProblem(new ValidationProblemDetails(vr.ToDictionary()));

        var entity = await _db.TiposBibliografia.FindAsync(id);
        if (entity == null) return NotFound();

        entity.Descripcion = dto.Descripcion;
        entity.Estado = dto.Estado;
        entity.ActualizadoEn = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.TiposBibliografia.FindAsync(id);
        if (entity == null) return NotFound();

        entity.Estado = false;
        entity.ActualizadoEn = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return NoContent();
    }
}