namespace BibliotecaAPEC.DTOs;
public class CreateBookDto
{
    public string Titulo { get; set; } = null!;
    public string Autor { get; set; } = null!;
    public string Categoria { get; set; } = null!;
    public string Idioma { get; set; } = "Español";
}
