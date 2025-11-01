namespace Unapec.Biblioteca.Core.Entities;

public class Empleado
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string TandaLabor { get; set; } = string.Empty; // Ej: "Matutina", "Vespertina", "Nocturna"
    public decimal PorcientoComision { get; set; }
    public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;
    public bool Estado { get; set; } = true;
}
