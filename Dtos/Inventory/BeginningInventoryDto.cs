using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Dtos.Inventory
{
    public class BeginningInventoryDto
    {
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public int InitialStockQuantity { get; set; }
        public decimal InitialCostPrice { get; set; }
        public decimal InitialRetailPrice { get; set; }
        public DateTime DateSet { get; set; }
    }
}