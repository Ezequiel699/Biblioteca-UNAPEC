using System.ComponentModel.DataAnnotations;

namespace Unapec.Biblioteca.Core.Entities;

public class Libro
{
    public int Id { get; set; }

    // Datos mínimos
    [Required, MaxLength(200)]
    public string Descripcion { get; set; } = default!;

    [MaxLength(50)]
    public string? SignaturaTopografica { get; set; }

    [MaxLength(20)]
    public string? ISBN { get; set; }

    // Relaciones (FKs)
    public int TipoBibliografiaId { get; set; }
    public int EditoraId { get; set; }
    public int AnioPublicacion { get; set; }
    public int CienciaId { get; set; }
    public int IdiomaId { get; set; }

    // Estado y timestamps
    public bool Estado { get; set; } = true;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? ActualizadoEn { get; set; }

    // Navegación
    public TipoBibliografia TipoBibliografia { get; set; } = default!;
    public Editora Editora { get; set; } = default!;
    public Ciencia? Ciencia { get; set; } = default!;
    public Idioma Idioma { get; set; } = default!;
    public ICollection<Autor> Autores { get; set; } = new List<Autor>();
}
