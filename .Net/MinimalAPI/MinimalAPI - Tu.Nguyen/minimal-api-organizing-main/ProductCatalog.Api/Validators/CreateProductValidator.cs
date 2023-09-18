using FluentValidation;
using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Validators;

public class CreateProductValidator : AbstractValidator<CreateProduct>
{
    public CreateProductValidator()
    {
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Price).NotEmpty().GreaterThanOrEqualTo(0);
        RuleFor(v => v.StockQuantity).GreaterThanOrEqualTo(0);
        RuleFor(v => v.CategoryId).NotEmpty();
    }
}