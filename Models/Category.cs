using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PacificaAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }  // e.g., "Pet Food - Cat Food"
        public string? Description { get; set; }  // Optional description of the category

        // Navigation property for the related products
        public ICollection<Product>? Products { get; set; }
    }
}