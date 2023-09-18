using System.ComponentModel;

namespace ProductCatalog.Api.Data.Enums;

public enum ERole
{
    [Description("Customer")] Customer = 0,
    [Description("Staff")] Staff,
    [Description("Admin")] Admin
}