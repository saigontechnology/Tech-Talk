using System;
using System.Collections.Generic;

namespace TStore.Shared.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public string UserName { get; set; }
        public double Total { get; set; }
        public double? ShipAmount { get; set; }
        public double? Discount { get; set; }

        public ICollection<ProductModel> Items { get; set; }
    }
}
