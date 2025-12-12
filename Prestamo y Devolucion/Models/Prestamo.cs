namespace BibliotecaAPEC.Models;
public class Prestamo
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public int LibroId { get; set; }
    public Book Libro { get; set; } = null!;
    public DateTime FechaPrestamo { get; set; } = DateTime.UtcNow;
    public DateTime? FechaDevolucion { get; set; }
    public bool Devuelto => FechaDevolucion != null;
}
