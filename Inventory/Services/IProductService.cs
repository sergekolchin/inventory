using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Inventory.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace Inventory.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task<Product> GetByIdAsync(int id);

        Task<Product> GetByIdWithWarehouseAsync(int id);

        Task<Product> AddAsync(Product entity);

        Task<Product> UpdateAsync(Product entity);

        Task<Product> DeleteAsync(int id);
    }
}
