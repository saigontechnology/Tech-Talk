using System;
using System.Collections.Generic;

namespace DapperSharing.Models;

public partial class OrderDetail
{
    public int IdOrder { get; set; }

    public int IdProduct { get; set; }

    public int TotalPrice { get; set; }

    public int TotalBasePrice { get; set; }

    public int QuantityPurchased { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
