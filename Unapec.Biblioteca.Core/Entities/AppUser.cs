namespace Unapec.Biblioteca.Core.Entities;

using System.ComponentModel.DataAnnotations;

public class AppUser
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = null!;

    // Campo para la cédula, SI decides que NO ES NECESARIO, lo quitamos
    // public string Cedula { get; set; } = null!; // <-- COMENTADO/ELIMINADO

    // Para login
    [Required]
    public string PasswordHash { get; set; } = null!;

    // admin, empleado, usuario
    public UserRole Rol { get; set; } = UserRole.usuario;

    public bool Estado { get; set; } = true;

    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? ActualizadoEn { get; set; }
}

public enum UserRole
{
    admin,
    empleado,
    usuario
}