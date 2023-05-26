using BasicLinQ.Entities;

namespace BasicLinQ
{
    public static class InitData
    {
        public static IEnumerable<Supplier> Suppliers => Enumerable.Range(1, 200)
            .Select(i => new Supplier { Id = i, Name = $"supplier {i}" });

        public static IEnumerable<Product> Products => new[]
        {
            new Product { Id = 1, Name = "apple", CategoryId = 1, SupplierId = 1 },
            new Product { Id = 2, Name = "banana", CategoryId = 1, SupplierId = 1 },
            new Product { Id = 3, Name = "orange", CategoryId = 1, SupplierId = 1 },
            new Product { Id = 4, Name = "grape", CategoryId = 1, SupplierId = 1 },
            new Product { Id = 5, Name = "mango", CategoryId = 1, SupplierId = 1 },
            new Product { Id = 6, Name = "television", CategoryId = 2, SupplierId = 2 },
            new Product { Id = 7, Name = "laptop", CategoryId = 2, SupplierId = 2 },
            new Product { Id = 8, Name = "keyboard", CategoryId = 2, SupplierId = 2 },
            new Product { Id = 9, Name = "monitor", CategoryId = 2, SupplierId = 2 },
            new Product { Id = 10, Name = "knife", CategoryId = 3, SupplierId = 3 },
            new Product { Id = 11, Name = "spoon", CategoryId = 3, SupplierId = 3 },
            new Product { Id = 12, Name = "bowl", CategoryId = 3, SupplierId = 3 },
            new Product { Id = 13, Name = "chopsticks", CategoryId = 3, SupplierId = 3 },
        };

        public static IEnumerable<ProductCategory> Categories => new[]
        {
            new ProductCategory { Id = 1, Name = "fruit", SupplierId = 1 },
            new ProductCategory { Id = 2, Name = "electronic", SupplierId = 2 },
            new ProductCategory { Id = 3, Name = "household", SupplierId = 3 },
            new ProductCategory { Id = 4, Name = "misc", SupplierId = 4 },
        };
    }

    public class GetProductCountByCategoryView
    {
        public string CategoryId { get; set; }
        public int ProductCount { get; set; }
    }
}
