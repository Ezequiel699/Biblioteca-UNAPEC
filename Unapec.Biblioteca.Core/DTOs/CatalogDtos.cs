namespace Unapec.Biblioteca.Core.DTOs;

public record CatalogCreateDto(string Descripcion, bool Estado = true);
public record CatalogUpdateDto(string Descripcion, bool Estado);
public record CatalogResponseDto(int Id, string Descripcion, bool Estado);
