using System;
using System.Collections.Generic;

namespace DapperSharing.Models;

public partial class Shipper
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? Address { get; set; }

    public string? Avatar { get; set; }

    public int TypeAcc { get; set; }

    public int Salary { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
