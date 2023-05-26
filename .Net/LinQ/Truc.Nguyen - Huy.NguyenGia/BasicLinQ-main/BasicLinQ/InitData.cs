using BasicLinQ.Entities;

namespace BasicLinQ
{
    public static class InitData
    {
        public static IEnumerable<Supplier> Suppliers => Enumerable.Range(1, 200)
            .Select(i => new Supplier { Id = i, IsDeleted = false, Name = $"supplier {i}" });

        public static IEnumerable<Product> Products => new[]
        {
            new Product { Id = 1, IsDeleted = false, Name = "apple", CategoryId = 1, SupplierId = 1 },
            new Product { Id = 2, IsDeleted = false, Name = "banana", CategoryId = 1, SupplierId = 1 },
            new Product { Id = 3, IsDeleted = false, Name = "orange", CategoryId = 1, SupplierId = 1 },
            new Product { Id = 4, IsDeleted = false, Name = "grape", CategoryId = 1, SupplierId = 1 },
            new Product { Id = 5, IsDeleted = false, Name = "mango", CategoryId = 1, SupplierId = 1 },
            new Product { Id = 6, IsDeleted = false, Name = "television", CategoryId = 2, SupplierId = 2 },
            new Product { Id = 7, IsDeleted = false, Name = "laptop", CategoryId = 2, SupplierId = 2 },
            new Product { Id = 8, IsDeleted = false, Name = "keyboard", CategoryId = 2, SupplierId = 2 },
            new Product { Id = 9, IsDeleted = false, Name = "monitor", CategoryId = 2, SupplierId = 2 },
            new Product { Id = 10, IsDeleted = false, Name = "knife", CategoryId = 3, SupplierId = 3 },
            new Product { Id = 11, IsDeleted = false, Name = "spoon", CategoryId = 3, SupplierId = 3 },
            new Product { Id = 12, IsDeleted = false, Name = "bowl", CategoryId = 3, SupplierId = 3 },
            new Product { Id = 13, IsDeleted = false, Name = "chopsticks", CategoryId = 3, SupplierId = 3 },
        };

        public static IEnumerable<ProductCategory> Categories => new[]
        {
            new ProductCategory { Id = 1, IsDeleted = false, Name = "fruit", SupplierId = 1 },
            new ProductCategory { Id = 2, IsDeleted = false, Name = "electronic", SupplierId = 2 },
            new ProductCategory { Id = 3, IsDeleted = false, Name = "household", SupplierId = 3 },
            new ProductCategory { Id = 4, IsDeleted = false, Name = "misc", SupplierId = 4 },
        };
    }

}
