
namespace BasicLinQ.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }

        public virtual ProductCategory Category { get; set; }
        public virtual Supplier Supplier { get; set; }

        public override string ToString() => $"Product: {Name}";
    }
}
