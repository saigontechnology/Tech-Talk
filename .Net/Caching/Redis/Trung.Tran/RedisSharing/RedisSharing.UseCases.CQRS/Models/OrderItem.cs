namespace RedisSharing.UseCases.CQRS.Models
{
    public class OrderItem
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string OrderId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }

        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}
