namespace BibliotecaAPEC.DTOs;
public class PrestamoDto
{
    public int Id { get; set; }
    public UsuarioDto Usuario { get; set; } = null!;
    public BookDto Libro { get; set; } = null!;
    public DateTime FechaPrestamo { get; set; }
    public DateTime? FechaDevolucion { get; set; }
    public bool Devuelto { get; set; }
}
