using C_11_Demo.Models;

namespace C_11_Demo.Pieces.Validator;


class MockProductValidate
{
    public bool Validate()
    {
        return true;
    }

    public bool Validate(Product data)
    {
        return data.Id.HasValue
            && (!string.IsNullOrEmpty(data.Name) || data.Age > 0);
    }
}

internal class MyValidationAttribute<T> : Attribute where T : IValidator, new()
{
    public T Validator { get; init; }

    public MyValidationAttribute()
    {
        Validator = Activator.CreateInstance<T>();
    }
}