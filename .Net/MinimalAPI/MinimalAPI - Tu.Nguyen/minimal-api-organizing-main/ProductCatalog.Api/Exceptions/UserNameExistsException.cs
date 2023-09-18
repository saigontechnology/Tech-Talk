namespace ProductCatalog.Api.Exceptions;

public class UserNameExistsException : Exception
{
    public UserNameExistsException(string userName)
        : base($"UserName: {userName} already exists.")
    {
    }
}