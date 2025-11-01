using FluentValidation;
using Unapec.Biblioteca.Core.DTOs;
using System.Text.RegularExpressions;

namespace Unapec.Biblioteca.Core.Validation;

public static class CedulaUtils
{
    private static readonly Regex _digits = new Regex(@"\D", RegexOptions.Compiled);
    public static string OnlyDigits(string? input) => input is null ? string.Empty : _digits.Replace(input, "");
}

public class UsuarioCreateValidator : AbstractValidator<UsuarioCreateDto>
{
    public UsuarioCreateValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Cedula)
            .NotEmpty()
            .Must(c => CedulaUtils.OnlyDigits(c).Length == 11)
            .WithMessage("'Cedula' debe tener 11 dígitos (con o sin guiones).");

        RuleFor(x => x.NoCarnet)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.TipoPersona)
            .NotEmpty()
            .Must(x => x.Equals("fisica", StringComparison.OrdinalIgnoreCase) ||
                       x.Equals("juridica", StringComparison.OrdinalIgnoreCase))
            .WithMessage("TipoPersona debe ser 'Fisica' o 'Juridica'.");
    }
}

public class UsuarioUpdateValidator : AbstractValidator<UsuarioUpdateDto>
{
    public UsuarioUpdateValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.Cedula)
            .NotEmpty()
            .Must(c => CedulaUtils.OnlyDigits(c).Length == 11)
            .WithMessage("'Cedula' debe tener 11 dígitos (con o sin guiones).");

        RuleFor(x => x.NoCarnet)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.TipoPersona)
            .NotEmpty()
            .Must(x => x.Equals("fisica", StringComparison.OrdinalIgnoreCase) ||
                       x.Equals("juridica", StringComparison.OrdinalIgnoreCase))
            .WithMessage("TipoPersona debe ser 'Fisica' o 'Juridica'.");
    }
}
