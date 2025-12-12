namespace BibliotecaAPEC.DTOs;
public class BookDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string Autor { get; set; } = null!;
    public string Categoria { get; set; } = null!;
    public string Idioma { get; set; } = null!;
    public bool Disponible { get; set; }
}
