using FluentValidation;
using Unapec.Biblioteca.Core.DTOs;

namespace Unapec.Biblioteca.Core.Validation
{
    public class AutorCreateValidator : AbstractValidator<AutorCreateDto>
    {
        public AutorCreateValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .Length(2, 120).WithMessage("El nombre debe tener entre 2 y 120 caracteres.");

            RuleFor(x => x.PaisOrigen)
                .MaximumLength(80).WithMessage("El país de origen no debe exceder 80 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.PaisOrigen));

            RuleFor(x => x.IdiomaNativoId)
                .GreaterThan(0).WithMessage("Debe indicar un idioma nativo válido.");
        }
    }

    public class AutorUpdateValidator : AbstractValidator<AutorUpdateDto>
    {
        public AutorUpdateValidator()
        {
            // OJO: el Id viene en la ruta, por eso no se valida aquí.

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .Length(2, 120).WithMessage("El nombre debe tener entre 2 y 120 caracteres.");

            RuleFor(x => x.PaisOrigen)
                .MaximumLength(80).WithMessage("El país de origen no debe exceder 80 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.PaisOrigen));

            RuleFor(x => x.IdiomaNativoId)
                .GreaterThan(0).WithMessage("Debe indicar un idioma nativo válido.");
        }
    }
}
