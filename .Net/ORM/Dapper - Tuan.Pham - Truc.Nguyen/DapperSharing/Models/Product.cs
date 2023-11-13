using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace DapperSharing.Models;

[Table("production.products")]
public partial class Product
{
    [Key]
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public int BrandId { get; set; }

    public int CategoryId { get; set; }

    public short ModelYear { get; set; }

    public decimal ListPrice { get; set; }

    [Write(false)]
    [Computed]
    public virtual Brand Brand { get; set; }

    [Write(false)]
    [Computed]
    public virtual Category Category { get; set; }

    [Write(false)]
    [Computed]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [Write(false)]
    [Computed]
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
