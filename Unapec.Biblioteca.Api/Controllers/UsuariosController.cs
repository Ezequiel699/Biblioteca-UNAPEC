using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unapec.Biblioteca.Core.DTOs;
using Unapec.Biblioteca.Core.Entities;
using Unapec.Biblioteca.Infrastructure.Data;
using FluentValidation;

namespace Unapec.Biblioteca.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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
}