using System;
using System.Collections.Generic;

namespace DapperSharing.Models;

public partial class Product
{
    public int Id { get; set; }

    public string ProductName { get; set; };

    public int Quantity { get; set; }

    public int Size { get; set; }

    public int IdProductDetail { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ProductDetail ProductDetail { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
