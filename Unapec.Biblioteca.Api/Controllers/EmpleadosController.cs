using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unapec.Biblioteca.Core.DTOs;
using Unapec.Biblioteca.Core.Entities;
using Unapec.Biblioteca.Infrastructure.Data;
using FluentValidation;

namespace Unapec.Biblioteca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmpleadosController : ControllerBase
{
    private readonly BibliotecaDbContext _context;
    private readonly IValidator<EmpleadoCreateDto> _createValidator;
    private readonly IValidator<EmpleadoUpdateDto> _updateValidator;

    public EmpleadosController(BibliotecaDbContext context,
                               IValidator<EmpleadoCreateDto> createValidator,
                               IValidator<EmpleadoUpdateDto> updateValidator)
    {
        _context = context;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet(Name = "GetEmpleados")]
    public async Task<ActionResult<object>> GetEmpleados([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] bool? estado = null)
    {
        var query = _context.Empleados.AsQueryable();
        if (estado.HasValue) query = query.Where(e => e.Estado == estado);

        var total = query.Count();
        var items = await query.OrderBy(e => e.Nombre)
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

    [HttpGet("{id:int}", Name = "GetEmpleadoById")]
    public async Task<ActionResult<Empleado>> GetEmpleado(int id)
    {
        var empleado = await _context.Empleados.FindAsync(id);
        if (empleado is null) return NotFound();
        return Ok(empleado);
    }

    [HttpPost(Name = "CreateEmpleado")]
    public async Task<ActionResult<Empleado>> CreateEmpleado(EmpleadoCreateDto dto)
    {
        var vr = await _createValidator.ValidateAsync(dto);
        if (!vr.IsValid) 
            return ValidationProblem(new ValidationProblemDetails(vr.ToDictionary()));

        var entity = new Empleado
        {
            Nombre = dto.Nombre,
            Cedula = dto.Cedula,
            TandaLabor = dto.TandaLabor,
            PorcientoComision = dto.PorcientoComision,
            FechaIngreso = dto.FechaIngreso,
            Estado = dto.Estado
        };

        _context.Empleados.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEmpleado), new { id = entity.Id }, entity);
    }

    [HttpPut("{id:int}", Name = "UpdateEmpleado")]
    public async Task<IActionResult> UpdateEmpleado(int id, EmpleadoUpdateDto dto)
    {
        var vr = await _updateValidator.ValidateAsync(dto);
        if (!vr.IsValid) 
            return ValidationProblem(new ValidationProblemDetails(vr.ToDictionary()));

        var entity = await _context.Empleados.FindAsync(id);
        if (entity is null) return NotFound();

        entity.Nombre = dto.Nombre;
        entity.Cedula = dto.Cedula;
        entity.TandaLabor = dto.TandaLabor;
        entity.PorcientoComision = dto.PorcientoComision;
        entity.FechaIngreso = dto.FechaIngreso;
        entity.Estado = dto.Estado;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Soft delete
    [HttpDelete("{id:int}", Name = "DeleteEmpleado")]
    public async Task<IActionResult> DeleteEmpleado(int id)
    {
        var entity = await _context.Empleados.FindAsync(id);
        if (entity is null) return NotFound();
        entity.Estado = false;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}