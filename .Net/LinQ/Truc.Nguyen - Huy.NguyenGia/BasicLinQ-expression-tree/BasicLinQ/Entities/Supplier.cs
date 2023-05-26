namespace BasicLinQ.Entities
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<Product> Products { get; set; }
        public virtual IEnumerable<ProductCategory> ProductCategories { get; set; }

        public override string ToString() => $"Supplier: {Name}";
    }
}
