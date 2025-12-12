using System.ComponentModel.DataAnnotations;
using Unapec.Biblioteca.Core.Entities;

namespace Unapec.Biblioteca.Core.DTOs;

public record AppUserUpdateDto(
    [Required] string Nombre,
    [Required] UserRole Rol,
    string? Password // Opcional, si se quiere cambiar la contraseña
);