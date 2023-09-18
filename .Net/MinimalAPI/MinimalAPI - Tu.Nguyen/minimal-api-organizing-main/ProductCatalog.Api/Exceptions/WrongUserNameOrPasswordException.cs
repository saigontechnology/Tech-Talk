namespace ProductCatalog.Api.Exceptions;

public class WrongUserNameOrPassword : Exception
{
    public WrongUserNameOrPassword()
        : base("Wrong user name or password.")
    {
    }
}