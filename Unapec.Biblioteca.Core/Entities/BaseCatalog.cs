namespace Unapec.Biblioteca.Core.Entities;

public abstract class BaseCatalog
{
    public int Id { get; set; }
    public required string Descripcion { get; set; }
    public bool Estado { get; set; } = true;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? ActualizadoEn { get; set; }
}
