using System;
using System.Collections.Generic;

namespace DapperSharing.Models;

public partial class ProductDetail
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Pic1 { get; set; }

    public string? Pic2 { get; set; }

    public string? Pic3 { get; set; }

    public string? Description { get; set; }

    public int Price { get; set; }

    public int Status { get; set; }

    public int IdProductType { get; set; }

    public bool IsDeleted { get; set; }

    public int BasePrice { get; set; }

    public virtual ICollection<FavoriteProduct> FavoriteProducts { get; set; } = new List<FavoriteProduct>();

    public virtual ProductType ProductType { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<ReviewDetail> ReviewDetails { get; set; } = new List<ReviewDetail>();
}
