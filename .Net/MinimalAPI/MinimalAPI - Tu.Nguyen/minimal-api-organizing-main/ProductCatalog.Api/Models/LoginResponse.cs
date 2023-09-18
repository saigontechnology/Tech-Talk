namespace ProductCatalog.Api.Models;

public class LoginResponse
{
    public UserInformation UserInformation { get; set; }
    public string Token { get; set; }
}