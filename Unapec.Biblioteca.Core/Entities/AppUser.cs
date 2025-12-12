namespace Unapec.Biblioteca.Core.Entities;

public class AppUser
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;
    public string Cedula { get; set; } = null!;

    // Para login
    public string UserName { get; set; } = null!;
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