using FluentValidation;
using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProduct>
{
    public UpdateProductValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Price).NotEmpty().GreaterThanOrEqualTo(0);
        RuleFor(v => v.StockQuantity).GreaterThanOrEqualTo(0);
        RuleFor(v => v.CategoryId).NotEmpty();
    }
}