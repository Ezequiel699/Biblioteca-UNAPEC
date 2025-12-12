using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Unapec.Biblioteca.Core.DTOs;
using Unapec.Biblioteca.Core.Entities;
using Unapec.Biblioteca.Infrastructure.Data;

namespace Unapec.Biblioteca.Api.Controllers;

[ApiController]
[Route("api/[controller]")] // La ruta base será api/auth/
public class AuthController : ControllerBase
{
    private readonly BibliotecaDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(BibliotecaDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Buscar por UserName
        var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Nombre == dto.Name);

        if (user == null || !user.Estado)
        {
            return Unauthorized(); // Combinamos la verificación de existencia y estado
        }

        // Verificar la contraseña (usa BCrypt o similar en producción)
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return Unauthorized(); // Contraseña incorrecta
        }

        var token = GenerateJwtToken(user);

        return Ok(new
        {
            token = token,
            user = new { user.Id, user.Nombre, rol = user.Rol.ToString() } // Incluir UserName en la respuesta
        });
    }

    // ==================== ENDPOINTS MODIFICADOS TEMPORALMENTE (sin Cedula) ====================

    // GET: api/auth/appusers
    [HttpGet("appusers")]
    [Authorize] // Ahora requiere autenticación para ver la lista
    public async Task<ActionResult<IEnumerable<AppUser>>> GetAppUsers(int page = 1, int pageSize = 20, string? q = null, bool? estado = null)
    {
        // Filtrar por búsqueda (q) y estado
        var query = _context.AppUsers.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
        {
            // Buscar en Nombre y UserName
            query = query.Where(u => u.Nombre.Contains(q));
        }
        if (estado.HasValue)
        {
            query = query.Where(u => u.Estado == estado.Value);
        }

        // Aplicar paginación
        var total = await query.CountAsync();
        var users = await query
            .OrderBy(u => u.Nombre) // Ordenar por nombre
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new { u.Id, u.Nombre,u.Rol, u.Estado, u.CreadoEn }) // Proyectar solo los campos necesarios
            .ToListAsync();

        return Ok(new
        {
            page,
            pageSize,
            total,
            items = users
        });
    }

    // POST: api/auth/appusers
    [HttpPost("appusers")] // Endpoint temporalmente sin autorización completa
    public async Task<ActionResult<AppUser>> CreateAppUser(AppUserCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Verificar si ya existen usuarios
        bool hayUsuarios = await _context.AppUsers.AnyAsync();

        // Si ya hay usuarios, se requiere autenticación y rol de admin
        if (hayUsuarios)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(); // No autenticado
            }

            if (!User.IsInRole("admin"))
            {
                return Forbid(); // Autenticado pero no es admin
            }
        }

        var newUser = new AppUser
        {
            Nombre = dto.Nombre,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), // Hash la contraseña antes de guardarla
            // Asignar rol: admin si es el primer usuario, usuario en caso contrario
            Rol = hayUsuarios ? UserRole.usuario : UserRole.admin,
            Estado = true // Activo por defecto
        };

        _context.AppUsers.Add(newUser);
        await _context.SaveChangesAsync();

        // Excluir PasswordHash de la respuesta
        return CreatedAtAction(
            nameof(GetAppUserById),
            new { id = newUser.Id },
            new { newUser.Id, newUser.Nombre,newUser.Rol, newUser.Estado, newUser.CreadoEn } // No incluir Cedula
        );
    }

    // GET: api/auth/appusers/{id}
    [HttpGet("appusers/{id:int}")] // Endpoint temporalmente sin autorización completa
    public async Task<ActionResult<object>> GetAppUserById(int id)
    {
        // Verificar si ya existen usuarios
        bool hayUsuarios = await _context.AppUsers.AnyAsync();

        // Si ya hay usuarios, se requiere autenticación
        if (hayUsuarios && !User.Identity.IsAuthenticated)
        {
            return Unauthorized(); // No autenticado
        }

        var user = await _context.AppUsers.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        // Excluir PasswordHash de la respuesta
        return Ok(new
        {
            user.Id,
            user.Nombre,
            user.Rol,
            user.Estado,
            user.CreadoEn,
            user.ActualizadoEn
        });
    }

    // PUT: api/auth/appusers/{id}
    [HttpPut("appusers/{id:int}")] // Endpoint temporalmente sin autorización completa
    public async Task<IActionResult> UpdateAppUser(int id, AppUserUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Verificar si ya existen usuarios
        bool hayUsuarios = await _context.AppUsers.AnyAsync();

        // Si ya hay usuarios, se requiere autenticación
        if (hayUsuarios && !User.Identity.IsAuthenticated)
        {
            return Unauthorized(); // No autenticado
        }

        var existingUser = await _context.AppUsers.FindAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        // Verificar si ya existen usuarios y el usuario actual no es admin
        if (hayUsuarios && !User.IsInRole("admin"))
        {
            // Solo el usuario mismo puede actualizarse, y no puede cambiar su rol ni ser desactivado
            if (User.FindFirst(ClaimTypes.NameIdentifier)?.Value != existingUser.Id.ToString())
            {
                return Forbid(); // No es el usuario dueño del recurso
            }


        }


        existingUser.Nombre = dto.Nombre;
        // Solo actualizar contraseña si se provee en el DTO
        if (!string.IsNullOrEmpty(dto.Password))
        {
            existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }

        existingUser.ActualizadoEn = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.AppUsers.Any(u => u.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/auth/appusers/{id}
    [HttpDelete("appusers/{id:int}")] // Endpoint temporalmente sin autorización completa
    public async Task<IActionResult> DeleteAppUser(int id)
    {
        // Verificar si ya existen usuarios
        bool hayUsuarios = await _context.AppUsers.AnyAsync();

        // Si ya hay usuarios, se requiere autenticación y rol de admin
        if (hayUsuarios)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(); // No autenticado
            }

            if (!User.IsInRole("admin"))
            {
                return Forbid(); // Autenticado pero no es admin
            }
        }

        var user = await _context.AppUsers.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        // No se puede borrar al primer usuario si ya existen otros (evita dejar la app sin admins)
        if (!hayUsuarios) // Si no hay otros usuarios (es el primero)
        {
            var totalUsers = await _context.AppUsers.CountAsync();
            if (totalUsers == 1) // Es el único usuario
            {
                return Forbid(); // No se puede eliminar el único usuario
            }
        }

        // Soft delete
        user.Estado = false;
        user.ActualizadoEn = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // ==================== FIN MODIFICACIONES TEMPORALMENTE ====================

    private string GenerateJwtToken(AppUser user)
    {
        var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key no configurado.");
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "unapec.biblioteca";

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Nombre),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Rol.ToString())
        };

        var tokenDescriptor = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}