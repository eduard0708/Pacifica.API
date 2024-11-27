using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Dtos.Inventory
{
    public class MonthlyInventoryDto
    {
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public int StockQuantity { get; set; }
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public DateTime Date { get; set; }
    }
}