using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Dtos.BranchProduct
{
    public class BranchProductDto
    {
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public int StockQuantity { get; set; }
        public string SKU { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}