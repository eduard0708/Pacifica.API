using System.ComponentModel.DataAnnotations;

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

        [Required] 
        public DateTime DateAdded { get; set; }

        // Date when the product was last updated
        public DateTime LastUpdated { get; set; }

        // Reorder level for the product (stock threshold for reordering)
        [Required]  // Ensures ReorderLevel is provided
        public int ReorderLevel { get; set; }

        // Minimum stock level for the product
        [Required]  // Ensures MinStockLevel is provided
        public int MinStockLevel { get; set; }

        
        [Required]  // Ensures ProductStatus is provided
        [StringLength(50)]  // Limits the length of ProductStatus

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
        
    }
}
