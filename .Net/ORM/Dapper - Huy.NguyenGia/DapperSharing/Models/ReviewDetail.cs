using System;
using System.Collections.Generic;

namespace DapperSharing.Models;

public partial class ReviewDetail
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public DateTime ReviewTime { get; set; }

    public int IdCustomer { get; set; }

    public int IdProductDetail { get; set; }

    public bool IsDeleted { get; set; }

    public int ReviewStatus { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ProductDetail ProductDetail { get; set; } = null!;
}
