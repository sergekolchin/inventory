using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Inventory.Controllers;
using Inventory.Data;
using Inventory.Models;
using Inventory.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Inventory.Tests
{
    public class ProductsControllerTest
    {
        private readonly ProductsController _controller;
        private readonly ApplicationDbContext _dbContext;

        public ProductsControllerTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _dbContext = new ApplicationDbContext(optionsBuilder);
            _dbContext.Warehouses.Add(new Warehouse { Id = 1, Name = "Main" });
            _dbContext.Products.Add(new Product { Id = 1, Name = "Apple", Type = "Food", ExpiryDate = DateTime.UtcNow.AddDays(30).Date, WarehouseId = 1 });
            _dbContext.Products.Add(new Product { Id = 2, Name = "Banana", Type = "Food", ExpiryDate = DateTime.UtcNow.AddDays(30).Date, WarehouseId = 1 });
            _dbContext.SaveChanges();

            var notifyService = new Mock<INotifyService>();

            var warehouseRepositoryLog = new Mock<ILogger<IRepository<Warehouse>>>();
            var warehouseRepository = new Repository<Warehouse>(_dbContext, warehouseRepositoryLog.Object);
            var warehouseService = new WarehouseService(warehouseRepository);

            var productRepositoryLog = new Mock<ILogger<IRepository<Product>>>();
            var productRepository = new Repository<Product>(_dbContext, productRepositoryLog.Object);
            var productService = new ProductService(productRepository);

            var productsControllerLog = new Mock<ILogger<ProductsController>>();

            _controller = new ProductsController(productService, warehouseService, notifyService.Object, productsControllerLog.Object);
        }

        [Fact]
        public async Task Products_Get_All()
        {
            // Arrange
            var list = new List<Product>
            {
                new Product { Id = 1, Name = "Apple", Type = "Food", ExpiryDate = DateTime.UtcNow.AddDays(30).Date, WarehouseId = 1},
                new Product { Id = 2, Name = "Banana", Type = "Food", ExpiryDate = DateTime.UtcNow.AddDays(30).Date, WarehouseId = 1}
            };

            // Act
            var result = (await _controller.GetProducts()).ToList();

            //Assert
            var products = result.Should().BeAssignableTo<IEnumerable<Product>>().Subject;
            products.Count().Should().Be(2);
            result.Should().BeEquivalentTo(list, options => options.Excluding(x => x.Warehouse));
        }


        [Fact]
        public async Task Products_Get_Specific()
        {
            // Act
            var actionResult = await _controller.GetProduct(1);

            //Assert
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            var product = okResult.Value.Should().BeAssignableTo<Product>().Subject;
            product.Id.Should().Be(1);
            product.Name.Should().Be("Apple");
            product.Warehouse.Name.Should().Be("Main");
        }

        [Fact]
        public async Task Product_Add()
        {
            // Arrange
            var newProduct = new Product
            {
                Id = 3,
                Name = "Orange",
                Type = "Food",
                ExpiryDate = DateTime.UtcNow.AddDays(30),
                WarehouseId = 1
            };

            // Act
            var result = await _controller.PostProduct(newProduct);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var product = okResult.Value.Should().BeAssignableTo<Product>().Subject;
            product.Id.Should().Be(3);
            product.Name.Should().Be("Orange");
        }

        [Fact]
        public async Task Product_Update()
        {
            // Arrange
            var product = await _dbContext.Products.FindAsync(1);
            product.Name = "Updated";

            // Act
            var result = await _controller.PutProduct(1, product);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var updatedProduct = okResult.Value.Should().BeAssignableTo<Product>().Subject;
            updatedProduct.Name.Should().Be("Updated");
        }

        [Fact]
        public async Task Product_Delete()
        {
            // Act
            var actionResult = await _controller.DeleteProduct(1);

            //Assert
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            var product = okResult.Value.Should().BeAssignableTo<Product>().Subject;
            product.Id.Should().Be(1);
        }
    }
}
