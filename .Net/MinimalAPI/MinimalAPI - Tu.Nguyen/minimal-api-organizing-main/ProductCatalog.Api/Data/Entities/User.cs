using ProductCatalog.Api.Data.Enums;

namespace ProductCatalog.Api.Data.Entities;

public class User : BaseEntity
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public ERole Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}