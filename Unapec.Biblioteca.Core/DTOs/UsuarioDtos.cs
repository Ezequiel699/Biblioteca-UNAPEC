namespace Unapec.Biblioteca.Core.DTOs;

public record UsuarioCreateDto(
    string Nombre,
    string Cedula,
    string NoCarnet,
    string TipoPersona, // "fisica" | "juridica"
    bool Estado = true
);

public record UsuarioUpdateDto(
    string Nombre,
    string Cedula,
    string NoCarnet,
    string TipoPersona,
    bool Estado = true
);
