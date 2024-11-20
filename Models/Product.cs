using System.ComponentModel.DataAnnotations;
using Pacifica.API.Models.GlobalAuditTrails;

namespace Pacifica.API.Models
{
    public class Product : AuditDetails
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]  // Limits the length of the SKU
        public string SKU { get; set; } = string.Empty;
        
        // Foreign Key: Relates the product to its category
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Foreign Key: Relates the product to its supplier
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        // Navigation property for related stock transactions (inventory movements)
        public ICollection<StockInOut>? StockInOuts { get; set; }

        // Navigation property for related branch products (branch-specific prices and stock)
        public ICollection<BranchProduct>? BranchProducts { get; set; }

        // Navigation property for 
        public ICollection<ProductAuditTrail>? ProductAuditTrails { get; set; }
    }
}
