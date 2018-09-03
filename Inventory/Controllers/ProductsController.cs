using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Helpers;
using Inventory.Models;
using Inventory.Services;
using Microsoft.Extensions.Logging;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IWarehouseService _warehouseService;
        private readonly INotifyService _notifyService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, IWarehouseService warehouseService,
            INotifyService notifyService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _warehouseService = warehouseService;
            _notifyService = notifyService;
            _logger = logger;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            _logger.LogInformation("GetProducts()");
            return await _productService.GetAllAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] int id)
        {
            _logger.LogInformation($"GetProduct({id})");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"GetProduct({id}) invalid ModelState: {ModelState.ErrorsToString()}");
                return BadRequest(ModelState);
            }

            var product = await _productService.GetByIdWithWarehouseAsync(id);

            if (product == null)
            {
                _logger.LogWarning($"GetProduct({id}) not found");
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            _logger.LogInformation($"PutProduct({id})");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"PutProduct({id}) invalid ModelState: {ModelState.ErrorsToString()}");
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                _logger.LogWarning($"PutProduct({id}) {id} != {product.Id}");
                return BadRequest();
            }

            var warehouse = _warehouseService.GetByIdAsync(product.WarehouseId);
            if (warehouse == null)
            {
                _logger.LogWarning($"PutProduct({id}) Warehouse with Id: {product.WarehouseId} not found");
            }

            try
            {
                await _productService.UpdateAsync(product);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, $"PutProduct({id}) DbUpdateConcurrencyException");
                throw;
            }

            return Ok(await _productService.GetByIdWithWarehouseAsync(product.Id));
        }

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            _logger.LogInformation($"PostProduct({product.Name})");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"PostProduct() invalid ModelState: {ModelState.ErrorsToString()}");
                return BadRequest(ModelState);
            }

            await _productService.AddAsync(product);

            return Ok(await _productService.GetByIdWithWarehouseAsync(product.Id));
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            _logger.LogInformation($"DeleteProduct({id})");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"DeleteProduct({id}) invalid ModelState: {ModelState.ErrorsToString()}");
                return BadRequest(ModelState);
            }

            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning($"DeleteProduct({id}) not found");
                return NotFound();
            }

            await _productService.DeleteAsync(id);

            // Notify about the sale
            // No waiting for completion
            _notifyService?.ProductSold(product);

            return Ok(product);
        }
    }
}