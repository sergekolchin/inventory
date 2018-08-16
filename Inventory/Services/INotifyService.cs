using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Models;

namespace Inventory.Services
{
    public interface INotifyService
    {
        Task Expired();
        Task ProductSold(Product product);
    }
}
