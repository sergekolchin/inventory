using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Inventory.Data;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Inventory.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;

        public ProductService(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _repository.GetAsync(include: x => x.Include(z => z.Warehouse));
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _repository.GetFirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Product> GetByIdWithWarehouseAsync(int id)
        {
            return await _repository.GetFirstOrDefaultAsync(x => x.Id == id, include: x => x.Include(z => z.Warehouse));
        }

        public async Task<Product> AddAsync(Product entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<Product> UpdateAsync(Product entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<Product> DeleteAsync(int id)
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
