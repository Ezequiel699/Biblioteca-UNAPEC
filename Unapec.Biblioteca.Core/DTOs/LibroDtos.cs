using System.Collections.Generic;

namespace Unapec.Biblioteca.Core.DTOs;

public sealed class LibroCreateDto
{
    public string Descripcion { get; set; } = null!;
    public string? SignaturaTopografica { get; set; }
    public string? ISBN { get; set; }

    public int TipoBibliografiaId { get; set; }
    public int EditoraId { get; set; }
    public int AnioPublicacion { get; set; }
    public int CienciaId { get; set; }
    public int IdiomaId { get; set; }

    public bool Estado { get; set; } = true;

    // 👇 relación N..N con Autores (obligatoria al crear)
    public List<int> AutorIds { get; set; } = new();
}

public sealed class LibroUpdateDto
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;
    public string? SignaturaTopografica { get; set; }
    public string? ISBN { get; set; }

    public int TipoBibliografiaId { get; set; }
    public int EditoraId { get; set; }
    public int AnioPublicacion { get; set; }
    public int CienciaId { get; set; }
    public int IdiomaId { get; set; }

    public bool Estado { get; set; } = true;

    // 👇 puede venir vacía si no se desea cambiar autores,
    // pero la propiedad debe existir para compilar con Program.cs
    public List<int> AutorIds { get; set; } = new();
}
