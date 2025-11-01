namespace Unapec.Biblioteca.Core.DTOs;

public record AutorUpdateDto(
    string Nombre,
    string? PaisOrigen,
    int IdiomaNativoId,
    bool Estado
);
