using FluentValidation;
using Unapec.Biblioteca.Core.DTOs;

namespace Unapec.Biblioteca.Core.Validation;

public sealed class LibroCreateValidator : AbstractValidator<LibroCreateDto>
{
    public LibroCreateValidator()
    {
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SignaturaTopografica).MaximumLength(50);
        RuleFor(x => x.ISBN).MaximumLength(20);

        RuleFor(x => x.TipoBibliografiaId).GreaterThan(0);
        RuleFor(x => x.EditoraId).GreaterThan(0);
        RuleFor(x => x.AnioPublicacion).InclusiveBetween(1400, 2100);
        RuleFor(x => x.CienciaId).GreaterThan(0);
        RuleFor(x => x.IdiomaId).GreaterThan(0);

        // al crear, al menos 1 autor
        RuleFor(x => x.AutorIds)
            .NotNull()
            .Must(l => l.Count > 0)
            .WithMessage("Debe indicar al menos un autor.");
    }
}

public sealed class LibroUpdateValidator : AbstractValidator<LibroUpdateDto>
{
    public LibroUpdateValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);

        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SignaturaTopografica).MaximumLength(50);
        RuleFor(x => x.ISBN).MaximumLength(20);

        RuleFor(x => x.TipoBibliografiaId).GreaterThan(0);
        RuleFor(x => x.EditoraId).GreaterThan(0);
        RuleFor(x => x.AnioPublicacion).InclusiveBetween(1400, 2100);
        RuleFor(x => x.CienciaId).GreaterThan(0);
        RuleFor(x => x.IdiomaId).GreaterThan(0);

        // en Update permitimos lista vacía (no cambia autores),
        // si quieres obligar: descomenta la regla de Count > 0
        RuleFor(x => x.AutorIds).NotNull();
    }
}
