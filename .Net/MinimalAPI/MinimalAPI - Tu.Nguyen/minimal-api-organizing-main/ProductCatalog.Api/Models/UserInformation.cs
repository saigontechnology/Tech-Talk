using ProductCatalog.Api.Data.Enums;

namespace ProductCatalog.Api.Models;

public class UserInformation
{
    public ERole Role { get; set; }
    public string RoleDisplay => Role.ToString();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}