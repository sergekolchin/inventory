using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Inventory.Controllers;
using Inventory.Data;
using Inventory.Models;
using Inventory.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Inventory.Tests
{
    public class WarehousesControllerTest
    {
        private readonly WarehousesController _controller;
        private readonly ApplicationDbContext _dbContext;

        public WarehousesControllerTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            _dbContext = new ApplicationDbContext(optionsBuilder);
            _dbContext.Warehouses.Add(new Warehouse { Id = 1, Name = "Main" });
            _dbContext.Warehouses.Add(new Warehouse { Id = 2, Name = "Second" });
            _dbContext.SaveChanges();

            var log = new Mock<ILogger<WarehousesController>>();
            var repositoryLog = new Mock<ILogger<IRepository<Warehouse>>>();

            var repository = new Repository<Warehouse>(_dbContext, repositoryLog.Object);
            var service = new WarehouseService(repository);
            _controller = new WarehousesController(service, log.Object);
        }

        [Fact]
        public async Task Warehouses_Get_All()
        {
            // Arrange
            var list = new List<Warehouse>
            {
                new Warehouse { Id = 1, Name = "Main" },
                new Warehouse { Id = 2, Name = "Second" }
            };

            // Act
            var result = (await _controller.GetWarehouses()).ToList();

            //Assert
            var warehouses = result.Should().BeAssignableTo<IEnumerable<Warehouse>>().Subject;
            warehouses.Count().Should().Be(2);
            result.Should().BeEquivalentTo(list, options => options.Excluding(x => x.Products));
        }


        [Fact]
        public async Task Warehouses_Get_Specific()
        {
            // Act
            var actionResult = await _controller.GetWarehouse(1);

            //Assert
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            var warehouse = okResult.Value.Should().BeAssignableTo<Warehouse>().Subject;
            warehouse.Id.Should().Be(1);
            warehouse.Name.Should().Be("Main");
        }

        [Fact]
        public async Task Warehouse_Add()
        {
            // Arrange
            var newWarehouse = new Warehouse
            {
                Id = 3,
                Name = "Third"
            };

            // Act
            var result = await _controller.PostWarehouse(newWarehouse);

            // Assert
            var okResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            var warehouse = okResult.Value.Should().BeAssignableTo<Warehouse>().Subject;
            warehouse.Id.Should().Be(3);
            warehouse.Name.Should().Be("Third");
        }

        [Fact]
        public async Task Warehouse_Update()
        {
            // Arrange
            var warehouse = await _dbContext.Warehouses.FindAsync(1);
            warehouse.Name = "Updated";

            // Act
            var result = await _controller.PutWarehouse(1, warehouse);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var updatedWarehouse = okResult.Value.Should().BeAssignableTo<Warehouse>().Subject;
            updatedWarehouse.Name.Should().Be("Updated");
        }

        [Fact]
        public async Task Warehouse_Delete()
        {
            // Act
            var actionResult = await _controller.DeleteWarehouse(1);

            //Assert
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            var warehouse = okResult.Value.Should().BeAssignableTo<Warehouse>().Subject;
            warehouse.Id.Should().Be(1);
        }
    }
}
