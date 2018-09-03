using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Inventory.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace Inventory.Services
{
    public interface IWarehouseService
    {
        Task<IEnumerable<Warehouse>> GetAllAsync();

        Task<Warehouse> GetByIdAsync(int id);

        Task<Warehouse> AddAsync(Warehouse entity);

        Task<Warehouse> UpdateAsync(Warehouse entity);

        Task<Warehouse> DeleteAsync(int id);
    }
}
