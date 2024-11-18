using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Dtos.Product
{
    public class GetFilter_ProductsDto
    {
        public string? Category { get; set; }
        public string? SKU { get; set; }
        public string? ProductName { get; set; }
        public string? Status { get; set; }
    }
}