using System;

namespace TStore.Shared.Models
{
    public class ShipAppliedEvent
    {
        public Guid OrderId { get; set; }
        public double ShipAmount { get; set; }
    }
}
