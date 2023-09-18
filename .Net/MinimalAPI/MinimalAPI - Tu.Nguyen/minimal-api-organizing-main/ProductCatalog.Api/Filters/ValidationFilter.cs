using FluentValidation;

namespace ProductCatalog.Api.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    private IValidator<T> _validator;
    private readonly IEnumerable<IValidator<T>> _validators;

    public ValidationFilter(IEnumerable<IValidator<T>> validators)
    {
        _validators = validators;
    }

    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var contextObj = (T) context.Arguments.SingleOrDefault(arg => arg?.GetType() == typeof(T));

        if (contextObj is not null) 
        {
            var validationContext = new ValidationContext<T>(contextObj);

            var failures = _validators
                .Select(v => v.Validate(validationContext))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
                throw new Exceptions.ValidationException(failures);
        }

        return await next(context);
    }
}