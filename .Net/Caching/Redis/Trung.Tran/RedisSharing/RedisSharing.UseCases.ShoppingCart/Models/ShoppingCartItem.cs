namespace RedisSharing.UseCases.ShoppingCart.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ProductName { get; set; }
        public DateTimeOffset AddedTime { get; set; }

        public virtual Product Product { get; set; }
        public virtual AppUser User { get; set; }
    }
}
