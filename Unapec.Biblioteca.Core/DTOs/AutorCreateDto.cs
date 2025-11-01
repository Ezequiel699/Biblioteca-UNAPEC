namespace Unapec.Biblioteca.Core.DTOs;

public record AutorCreateDto(
    string Nombre,
    string? PaisOrigen,
    int IdiomaNativoId,
    bool Estado = true
);
