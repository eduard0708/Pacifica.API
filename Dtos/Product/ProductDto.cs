using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pacifica.API.Dtos.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public int StockQuantity { get; set; }
        public string? SKU { get; set; }
        public DateTime DateAdded { get; set; }
        public string? ProductStatus { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public bool IsActive { get; set; }
    }
}