using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PacificaAPI.Models
{
    public class BranchProduct
    {
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int StockQuantity { get; set; }
        public string SKU { get; set; } = string.Empty;
    }

}