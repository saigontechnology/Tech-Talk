using System;
using System.Collections.Generic;

namespace DapperSharing.Models;

public partial class ProductType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int IdBrand { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
}
