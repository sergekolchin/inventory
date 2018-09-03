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
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _warhouseService;
        private readonly ILogger<WarehousesController> _logger;

        public WarehousesController(IWarehouseService warhouseService, ILogger<WarehousesController> logger)
        {
            _warhouseService = warhouseService;
            _logger = logger;
        }

        // GET: api/Warehouses
        [HttpGet]
        public async Task<IEnumerable<Warehouse>> GetWarehouses()
        {
            _logger.LogInformation("GetWarehouses()");
            return await _warhouseService.GetAllAsync();
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

            var warehouse = await _warhouseService.GetByIdAsync(id);

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

            try
            {
                await _warhouseService.UpdateAsync(warehouse);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, $"PutWarehouse({id}) DbUpdateConcurrencyException");
                throw;
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

            await _warhouseService.AddAsync(warehouse);

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

            var warehouse = await _warhouseService.DeleteAsync(id);
            return Ok(warehouse);
        }
    }
}