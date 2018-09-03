using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Inventory.Data;
using Inventory.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace Inventory.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IRepository<Warehouse> _repository;

        public WarehouseService(IRepository<Warehouse> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Warehouse>> GetAllAsync()
        {
            return await _repository.GetAsync();
        }

        public async Task<Warehouse> GetByIdAsync(int id)
        {
            return await _repository.GetFirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Warehouse> AddAsync(Warehouse entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<Warehouse> UpdateAsync(Warehouse entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<Warehouse> DeleteAsync(int id)
        {
            var entity = await _repository.GetFirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                throw new NullReferenceException();
            }

            return await _repository.RemoveAsync(entity);
        }
    }
}
