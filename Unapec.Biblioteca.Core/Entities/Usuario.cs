namespace Unapec.Biblioteca.Core.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;    // 11 dígitos
    public string NoCarnet { get; set; } = string.Empty;  // único
    public string TipoPersona { get; set; } = string.Empty; // "fisica" | "juridica"
    public bool Estado { get; set; } = true;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? ActualizadoEn { get; set; }
}
