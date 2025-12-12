namespace BibliotecaAPEC.Utils;
public class PrestamoQueryParams
{
    public int? UsuarioId { get; set; }
    public string? Categoria { get; set; }
    public string? Idioma { get; set; }
    public DateTime? Desde { get; set; }
    public DateTime? Hasta { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
