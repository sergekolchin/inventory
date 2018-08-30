using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Data;
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
        private readonly ApplicationDbContext _context;
        private readonly INotifyService _notifyService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext context, INotifyService notifyService, ILogger<ProductsController> logger)
        {
            _context = context;
            _notifyService = notifyService;
            _logger = logger;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            _logger.LogInformation("GetProducts()");
            return await _context.Products.Include(x => x.Warehouse).ToListAsync();
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

            var product = await _context.Products.Include(x => x.Warehouse).FirstOrDefaultAsync(x => x.Id == id);

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

            var warehouse = _context.Warehouses.FindAsync(product.WarehouseId);
            if (warehouse == null)
            {
                _logger.LogWarning($"PutProduct({id}) Warehouse with Id: {product.WarehouseId} not found");
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProductExists(id))
                {
                    _logger.LogWarning(ex, $"PutProduct({id}) not found");
                    return NotFound();
                }
                else
                {
                    _logger.LogWarning(ex, $"PutProduct({id}) DbUpdateConcurrencyException");
                    throw;
                }
            }

            return Ok(await _context.Products.Include(x => x.Warehouse).FirstOrDefaultAsync(x => x.Id == product.Id));
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

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(await _context.Products.Include(x => x.Warehouse).FirstOrDefaultAsync(x => x.Id == product.Id));
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

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                _logger.LogWarning($"DeleteProduct({id}) not found");
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            // Notify about the sale
            // No waiting for completion
            _notifyService?.ProductSold(product);

            return Ok(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}