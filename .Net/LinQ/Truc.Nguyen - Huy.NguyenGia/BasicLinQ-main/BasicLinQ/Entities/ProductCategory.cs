namespace BasicLinQ.Entities
{
    public class ProductCategory : BaseEntity
    {
        public string Name { get; set; }
        public int SupplierId { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual IEnumerable<Product> Products { get; set; }

        public override string ToString() => $"Category: {Name}";
    }
}
