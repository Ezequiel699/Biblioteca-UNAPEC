using System.ComponentModel.DataAnnotations;
using Unapec.Biblioteca.Core.Entities;

namespace Unapec.Biblioteca.Core.DTOs;

public record AppUserCreateDto(
    [Required] string Nombre,
    [Required] UserRole Rol,
    [Required] string Password // En el DTO de creación, se recibe la contraseña en texto plano para ser hasheada
);