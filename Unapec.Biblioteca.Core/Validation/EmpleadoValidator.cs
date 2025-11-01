using FluentValidation;
using Unapec.Biblioteca.Core.DTOs;

namespace Unapec.Biblioteca.Core.Validation;

public class EmpleadoCreateValidator : AbstractValidator<EmpleadoCreateDto>
{
    public EmpleadoCreateValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Cedula).NotEmpty().Length(11);
        RuleFor(x => x.TandaLabor).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PorcientoComision).InclusiveBetween(0, 100);
        RuleFor(x => x.FechaIngreso).LessThanOrEqualTo(DateTime.UtcNow);
    }
}

public class EmpleadoUpdateValidator : AbstractValidator<EmpleadoUpdateDto>
{
    public EmpleadoUpdateValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Cedula).NotEmpty().Length(11);
        RuleFor(x => x.TandaLabor).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PorcientoComision).InclusiveBetween(0, 100);
        RuleFor(x => x.FechaIngreso).LessThanOrEqualTo(DateTime.UtcNow);
    }
}
