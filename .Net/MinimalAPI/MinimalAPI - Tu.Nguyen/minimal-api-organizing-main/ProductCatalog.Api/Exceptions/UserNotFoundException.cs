namespace ProductCatalog.Api.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string userName)
        : base($"User: {userName} was not found.")
    {
    }
}