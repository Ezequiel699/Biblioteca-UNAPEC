namespace Unapec.Biblioteca.Core.DTOs;

public record EmpleadoCreateDto(
    string Nombre,
    string Cedula,
    string TandaLabor,
    decimal PorcientoComision,
    DateTime FechaIngreso,
    bool Estado
);

public record EmpleadoUpdateDto(
    string Nombre,
    string Cedula,
    string TandaLabor,
    decimal PorcientoComision,
    DateTime FechaIngreso,
    bool Estado
);
