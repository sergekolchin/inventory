using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
