using System;
using System.Collections.Generic;

namespace DapperSharing.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateTime OrderDay { get; set; }

    public int StatusOrder { get; set; }

    public int IdCustomer { get; set; }

    public bool IsDeleted { get; set; }

    public int ExtraFee { get; set; }

    public int? IdShipper { get; set; }

    public string? Address { get; set; }

    public bool? IsPaid { get; set; }

    public int Payment { get; set; }

    public virtual Customer IdCustomerNavigation { get; set; } = null!;

    public virtual Shipper? IdShipperNavigation { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
