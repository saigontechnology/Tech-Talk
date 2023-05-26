namespace RedisSharing.UseCases.CQRS.Models
{
    public class Order
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public DateTimeOffset OrderedTime { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; }
    }
}
