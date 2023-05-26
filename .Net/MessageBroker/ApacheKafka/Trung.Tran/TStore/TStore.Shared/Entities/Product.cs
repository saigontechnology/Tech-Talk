using System;
using System.Collections.Generic;

namespace TStore.Shared.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
