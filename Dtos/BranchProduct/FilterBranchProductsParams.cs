using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Dtos.BranchProduct
{
    public class FilterBranchProductsParams
    {
        public int BranchId { get; set; }
        public string? ProductCategory { get; set; }
        public string? Sku { get; set; }
        public string? ProductName { get; set; }
    }
}