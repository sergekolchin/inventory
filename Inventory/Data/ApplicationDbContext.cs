using Microsoft.EntityFrameworkCore;
using Inventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Product> Products { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Warehouse>().HasData(new Warehouse { Id = 1, Name = "Main" });
            modelBuilder.Entity<Warehouse>().Property(x => x.CreatedOn).HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Warehouse>().Property(x => x.LastModifiedOn).HasDefaultValue(DateTime.UtcNow);

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Apple", Type = "Food", WarehouseId = 1, ExpiryDate = DateTime.UtcNow.AddDays(1) },
                new Product { Id = 2, Name = "Banana", Type = "Food", WarehouseId = 1, ExpiryDate = DateTime.UtcNow.AddDays(2) },
                new Product { Id = 3, Name = "Milk", Type = "Food", WarehouseId = 1, ExpiryDate = DateTime.UtcNow },
                new Product { Id = 4, Name = "Meat", Type = "Food", WarehouseId = 1, ExpiryDate = DateTime.UtcNow },
                new Product { Id = 5, Name = "Bread", Type = "Food", WarehouseId = 1, ExpiryDate = DateTime.UtcNow });

            modelBuilder.Entity<Product>().Property(x => x.CreatedOn).HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Product>().Property(x => x.LastModifiedOn).HasDefaultValue(DateTime.UtcNow);
        }
    }
}
