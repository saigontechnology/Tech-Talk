using System;
using System.Collections.Generic;

namespace DapperSharing.Models;

public partial class Brand
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public string? Pic { get; set; }

    public virtual ICollection<ProductType> ProductTypes { get; set; } = new List<ProductType>();
}
