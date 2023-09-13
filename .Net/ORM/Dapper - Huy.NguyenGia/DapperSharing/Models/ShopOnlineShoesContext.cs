using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DapperSharing.Models;

public partial class ShopOnlineShoesContext : DbContext
{
    private readonly string _connectionString;
    public ShopOnlineShoesContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public ShopOnlineShoesContext(DbContextOptions<ShopOnlineShoesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<FavoriteProduct> FavoriteProducts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductDetail> ProductDetails { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<ReviewDetail> ReviewDetails { get; set; }

    public virtual DbSet<Shipper> Shippers { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("Brand");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customer");
        });

        modelBuilder.Entity<FavoriteProduct>(entity =>
        {
            entity.ToTable("FavoriteProduct");

            entity.HasIndex(e => e.IdCustomer, "IX_FavoriteProduct_IdCustomer");

            entity.HasIndex(e => e.IdProductDetail, "IX_FavoriteProduct_IdProductDetail");

            entity.HasOne(d => d.Customer).WithMany(p => p.FavoriteProducts).HasForeignKey(d => d.IdCustomer);

            entity.HasOne(d => d.ProductDetail).WithMany(p => p.FavoriteProducts).HasForeignKey(d => d.IdProductDetail);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.HasIndex(e => e.IdCustomer, "IX_Order_IdCustomer");

            entity.HasIndex(e => e.IdShipper, "IX_Order_IdShipper");

            entity.Property(e => e.IsPaid)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.Orders).HasForeignKey(d => d.IdCustomer);

            entity.HasOne(d => d.IdShipperNavigation).WithMany(p => p.Orders).HasForeignKey(d => d.IdShipper);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.IdOrder, e.IdProduct });

            entity.ToTable("OrderDetail");

            entity.HasIndex(e => e.IdProduct, "IX_OrderDetail_IdProduct");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails).HasForeignKey(d => d.IdOrder);

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails).HasForeignKey(d => d.IdProduct);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.HasIndex(e => e.IdProductDetail, "IX_Product_IdProductDetail");

            entity.HasOne(d => d.ProductDetail).WithMany(p => p.Products).HasForeignKey(d => d.IdProductDetail);
        });

        modelBuilder.Entity<ProductDetail>(entity =>
        {
            entity.ToTable("ProductDetail");

            entity.HasIndex(e => e.IdProductType, "IX_ProductDetail_IdProductType");

            entity.HasOne(d => d.ProductType).WithMany(p => p.ProductDetails).HasForeignKey(d => d.IdProductType);
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.ToTable("ProductType");

            entity.HasIndex(e => e.IdBrand, "IX_ProductType_IdBrand");

            entity.HasOne(d => d.Brand).WithMany(p => p.ProductTypes).HasForeignKey(d => d.IdBrand);
        });

        modelBuilder.Entity<ReviewDetail>(entity =>
        {
            entity.ToTable("ReviewDetail");

            entity.HasIndex(e => e.IdCustomer, "IX_ReviewDetail_IdCustomer");

            entity.HasIndex(e => e.IdProductDetail, "IX_ReviewDetail_IdProductDetail");

            entity.HasOne(d => d.Customer).WithMany(p => p.ReviewDetails).HasForeignKey(d => d.IdCustomer);

            entity.HasOne(d => d.ProductDetail).WithMany(p => p.ReviewDetails).HasForeignKey(d => d.IdProductDetail);
        });

        modelBuilder.Entity<Shipper>(entity =>
        {
            entity.ToTable("Shipper");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
