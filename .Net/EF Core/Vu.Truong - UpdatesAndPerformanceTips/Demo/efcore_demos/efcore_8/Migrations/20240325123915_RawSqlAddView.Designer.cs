﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using efcore_demos.DataAccess;

#nullable disable

namespace efcore_demos.Migrations
{
    [DbContext(typeof(DemoDbContext))]
    [Migration("20240325123915_RawSqlAddView")]
    partial class RawSqlAddView
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("efcore_demos.Entities.InvoiceEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("InvoiceCode")
                        .HasMaxLength(1024)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1024)");

                    b.Property<string>("OrderCode")
                        .HasMaxLength(1024)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1024)");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetimeoffset");

                    b.ComplexProperty<Dictionary<string, object>>("Contact", "efcore_demos.Entities.InvoiceEntity.Contact#ContactEntity", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("FirstName")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.Property<string>("LastName")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.ComplexProperty<Dictionary<string, object>>("Address", "efcore_demos.Entities.InvoiceEntity.Contact#ContactEntity.Address#AddressEntity", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<string>("City")
                                        .HasMaxLength(1024)
                                        .IsUnicode(false)
                                        .HasColumnType("varchar(1024)");

                                    b2.Property<string>("Country")
                                        .HasMaxLength(1024)
                                        .IsUnicode(false)
                                        .HasColumnType("varchar(1024)");

                                    b2.Property<string>("Postcode")
                                        .HasMaxLength(1024)
                                        .IsUnicode(false)
                                        .HasColumnType("varchar(1024)");

                                    b2.Property<string>("Street")
                                        .HasMaxLength(1024)
                                        .IsUnicode(false)
                                        .HasColumnType("varchar(1024)");
                                });

                            b1.ComplexProperty<Dictionary<string, object>>("PhoneNumber", "efcore_demos.Entities.InvoiceEntity.Contact#ContactEntity.PhoneNumber#PhoneNumberEntity", b2 =>
                                {
                                    b2.IsRequired();

                                    b2.Property<int>("CountryCode")
                                        .HasColumnType("int");

                                    b2.Property<long>("Number")
                                        .HasColumnType("bigint");
                                });
                        });

                    b.HasKey("Id");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("efcore_demos.Entities.OrderEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasMaxLength(1024)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1024)");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetimeoffset");

                    b.ComplexProperty<Dictionary<string, object>>("BillingAddress", "efcore_demos.Entities.OrderEntity.BillingAddress#AddressEntity", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("City")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.Property<string>("Country")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.Property<string>("Postcode")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.Property<string>("Street")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("ShippingAddress", "efcore_demos.Entities.OrderEntity.ShippingAddress#AddressEntity", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("City")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.Property<string>("Country")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.Property<string>("Postcode")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.Property<string>("Street")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");
                        });

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("efcore_demos.Entities.ProductEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasMaxLength(1024)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1024)");

                    b.Property<string>("Name")
                        .HasMaxLength(1024)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1024)");

                    b.Property<string>("Price")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("efcore_demos.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("BirthDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .HasMaxLength(1024)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1024)");

                    b.Property<string>("Password")
                        .HasMaxLength(1024)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1024)");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("UserName")
                        .HasMaxLength(1024)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1024)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("efcore_demos.Entities.InvoiceEntity", b =>
                {
                    b.OwnsMany("efcore_demos.Entities.AddressEntity", "Addresses", b1 =>
                        {
                            b1.Property<int>("InvoiceEntityId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            b1.Property<string>("City")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.Property<string>("Country")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.Property<string>("Postcode")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.Property<string>("Street")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.HasKey("InvoiceEntityId", "Id");

                            b1.ToTable("Invoices");

                            b1.ToJson("Addresses");

                            b1.WithOwner()
                                .HasForeignKey("InvoiceEntityId");
                        });

                    b.Navigation("Addresses");
                });

            modelBuilder.Entity("efcore_demos.Entities.OrderEntity", b =>
                {
                    b.HasOne("efcore_demos.Entities.ProductEntity", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("efcore_demos.Entities.DeliveryDetailEntity", "DeliveryDetail", b1 =>
                        {
                            b1.Property<Guid>("OrderEntityId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Note")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.Property<decimal>("Tips")
                                .HasColumnType("decimal(18,2)");

                            b1.HasKey("OrderEntityId");

                            b1.ToTable("Orders");

                            b1.ToJson("DeliveryDetail");

                            b1.WithOwner()
                                .HasForeignKey("OrderEntityId");

                            b1.OwnsOne("efcore_demos.Entities.AddressEntity", "Address", b2 =>
                                {
                                    b2.Property<Guid>("DeliveryDetailEntityOrderEntityId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<string>("City")
                                        .HasMaxLength(1024)
                                        .IsUnicode(false)
                                        .HasColumnType("varchar(1024)");

                                    b2.Property<string>("Country")
                                        .HasMaxLength(1024)
                                        .IsUnicode(false)
                                        .HasColumnType("varchar(1024)");

                                    b2.Property<string>("Postcode")
                                        .HasMaxLength(1024)
                                        .IsUnicode(false)
                                        .HasColumnType("varchar(1024)");

                                    b2.Property<string>("Street")
                                        .HasMaxLength(1024)
                                        .IsUnicode(false)
                                        .HasColumnType("varchar(1024)");

                                    b2.HasKey("DeliveryDetailEntityOrderEntityId");

                                    b2.ToTable("Orders");

                                    b2.WithOwner()
                                        .HasForeignKey("DeliveryDetailEntityOrderEntityId");
                                });

                            b1.Navigation("Address");
                        });

                    b.Navigation("DeliveryDetail");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("efcore_demos.Entities.UserEntity", b =>
                {
                    b.OwnsOne("efcore_demos.Entities.AddressEntity", "Address", b1 =>
                        {
                            b1.Property<Guid>("UserEntityId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.Property<string>("Country")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.Property<string>("Postcode")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.Property<string>("Street")
                                .HasMaxLength(1024)
                                .IsUnicode(false)
                                .HasColumnType("varchar(1024)");

                            b1.HasKey("UserEntityId");

                            b1.ToTable("Users");

                            b1.ToJson("Address");

                            b1.WithOwner()
                                .HasForeignKey("UserEntityId");
                        });

                    b.Navigation("Address");
                });
#pragma warning restore 612, 618
        }
    }
}