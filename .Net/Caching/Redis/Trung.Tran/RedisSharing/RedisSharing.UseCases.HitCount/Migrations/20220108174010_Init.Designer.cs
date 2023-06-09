﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RedisSharing.UseCases.HitCount.SqlServerStore;

#nullable disable

namespace RedisSharing.UseCases.HitCount.Migrations
{
    [DbContext(typeof(HitcountContext))]
    [Migration("20220108174010_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("RedisSharing.UseCases.HitCount.Models.HitCountItem", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("Count")
                        .HasColumnType("bigint");

                    b.HasKey("Key");

                    b.ToTable("HitCountItems");
                });

            modelBuilder.Entity("RedisSharing.UseCases.HitCount.Models.IpAddress", b =>
                {
                    b.Property<string>("Ip")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Ip");

                    b.ToTable("IpAddresses");
                });
#pragma warning restore 612, 618
        }
    }
}
