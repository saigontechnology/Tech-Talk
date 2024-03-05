using C_11_Demo.Models;

namespace C_11_Demo.Pieces.Validator;
internal interface IValidator
{

}

internal interface IValidator<T> : IValidator
{
    bool Validate(T data);
}

internal abstract class BaseValidator<T> : IValidator<T>
{
    public abstract bool Validate(T data);
}


class AddProductValidator : BaseValidator<Product>
{
    public override bool Validate(Product data)
    {
        return !string.IsNullOrEmpty(data.Name) && data.Age > 0;
    }
}

class UpdateProductValidator : BaseValidator<Product>
{
    public override bool Validate(Product data)
    {
        return data.Id.HasValue
            && (!string.IsNullOrEmpty(data.Name) || data.Age > 0);
    }
}