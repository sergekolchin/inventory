﻿// <auto-generated />
using System;
using Inventory.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Inventory.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Inventory.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2018, 9, 3, 8, 6, 59, 852, DateTimeKind.Utc));

                    b.Property<DateTime?>("ExpiryDate");

                    b.Property<DateTime>("LastModifiedOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2018, 9, 3, 8, 6, 59, 852, DateTimeKind.Utc));

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Type");

                    b.Property<int>("WarehouseId");

                    b.HasKey("Id");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Products");

                    b.HasData(
                        new { Id = 1, CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ExpiryDate = new DateTime(2018, 9, 4, 8, 6, 59, 852, DateTimeKind.Utc), LastModifiedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), Name = "Apple", Type = "Food", WarehouseId = 1 },
                        new { Id = 2, CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ExpiryDate = new DateTime(2018, 9, 5, 8, 6, 59, 852, DateTimeKind.Utc), LastModifiedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), Name = "Banana", Type = "Food", WarehouseId = 1 },
                        new { Id = 3, CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ExpiryDate = new DateTime(2018, 9, 3, 8, 6, 59, 852, DateTimeKind.Utc), LastModifiedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), Name = "Milk", Type = "Food", WarehouseId = 1 },
                        new { Id = 4, CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ExpiryDate = new DateTime(2018, 9, 3, 8, 6, 59, 852, DateTimeKind.Utc), LastModifiedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), Name = "Meat", Type = "Food", WarehouseId = 1 },
                        new { Id = 5, CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ExpiryDate = new DateTime(2018, 9, 3, 8, 6, 59, 852, DateTimeKind.Utc), LastModifiedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), Name = "Bread", Type = "Food", WarehouseId = 1 }
                    );
                });

            modelBuilder.Entity("Inventory.Models.Warehouse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2018, 9, 3, 8, 6, 59, 852, DateTimeKind.Utc));

                    b.Property<DateTime>("LastModifiedOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2018, 9, 3, 8, 6, 59, 852, DateTimeKind.Utc));

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Warehouses");

                    b.HasData(
                        new { Id = 1, CreatedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), LastModifiedOn = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), Name = "Main" }
                    );
                });

            modelBuilder.Entity("Inventory.Models.Product", b =>
                {
                    b.HasOne("Inventory.Models.Warehouse", "Warehouse")
                        .WithMany("Products")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
