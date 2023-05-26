using System;

namespace TStore.Shared.Models
{
    public class PromotionAppliedEvent
    {
        public Guid OrderId { get; set; }
        public double Discount { get; set; }
    }
}
