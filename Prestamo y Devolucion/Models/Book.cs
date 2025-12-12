namespace BibliotecaAPEC.Models;
public class Book
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string Autor { get; set; } = null!;
    public string Categoria { get; set; } = null!; // Ciencia, Historia...
    public string Idioma { get; set; } = "Español";
    public bool Disponible { get; set; } = true;
}
