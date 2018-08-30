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
using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account;
using Microsoft.Extensions.Logging;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WarehousesController> _logger;

        public WarehousesController(ApplicationDbContext context, ILogger<WarehousesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Warehouses
        [HttpGet]
        public async Task<IEnumerable<Warehouse>> GetWarehouses()
        {
            _logger.LogInformation("GetWarehouses()");
            return await _context.Warehouses.ToListAsync();
        }

        // GET: api/Warehouses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWarehouse([FromRoute] int id)
        {
            _logger.LogInformation($"GetWarehouses({id})");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"GetWarehouses({id}) invalid ModelState: {ModelState.ErrorsToString()}");
                return BadRequest(ModelState);
            }

            var warehouse = await _context.Warehouses.FindAsync(id);

            if (warehouse == null)
            {
                _logger.LogWarning($"GetWarehouses({id}) not found");
                return NotFound();
            }

            return Ok(warehouse);
        }

        // PUT: api/Warehouses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWarehouse([FromRoute] int id, [FromBody] Warehouse warehouse)
        {
            _logger.LogInformation($"PutWarehouse({id})");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"PutWarehouse({id}) invalid ModelState: {ModelState.ErrorsToString()}");
                return BadRequest(ModelState);
            }

            if (id != warehouse.Id)
            {
                _logger.LogWarning($"PutWarehouse({id}) {id} != {warehouse.Id}");
                return BadRequest();
            }

            _context.Entry(warehouse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!WarehouseExists(id))
                {
                    _logger.LogWarning(ex, $"PutWarehouse({id}) not found");
                    return NotFound();
                }
                else
                {
                    _logger.LogWarning(ex, $"PutWarehouse({id}) DbUpdateConcurrencyException");
                    throw;
                }
            }

            return Ok(warehouse);
        }

        // POST: api/Warehouses
        [HttpPost]
        public async Task<IActionResult> PostWarehouse([FromBody] Warehouse warehouse)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"PostWarehouse({warehouse.Id}) invalid ModelState: {ModelState.ErrorsToString()}");
                return BadRequest(ModelState);
            }

            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWarehouse", new { id = warehouse.Id }, warehouse);
        }

        // DELETE: api/Warehouses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"DeleteWarehouse({id}) invalid ModelState: {ModelState.ErrorsToString()}");
                return BadRequest(ModelState);
            }

            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
            {
                _logger.LogWarning($"DeleteWarehouse({id}) not found");
                return NotFound();
            }

            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();

            return Ok(warehouse);
        }

        private bool WarehouseExists(int id)
        {
            return _context.Warehouses.Any(e => e.Id == id);
        }
    }
}