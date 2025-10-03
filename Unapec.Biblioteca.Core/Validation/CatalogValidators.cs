using FluentValidation;
using Unapec.Biblioteca.Core.DTOs;

public class CatalogCreateValidator : AbstractValidator<CatalogCreateDto>
{
    public CatalogCreateValidator()
    {
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(120);
    }
}

public class CatalogUpdateValidator : AbstractValidator<CatalogUpdateDto>
{
    public CatalogUpdateValidator()
    {
        RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(120);
    }
}
