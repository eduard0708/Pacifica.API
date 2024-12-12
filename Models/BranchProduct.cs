using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pacifica.API.Models.GlobalAuditTrails;
using Pacifica.API.Models.Inventory;

public class BranchProduct : AuditDetails
{

    [Required(ErrorMessage = "Branch ID is required.")]
    public int BranchId { get; set; }

    [Required(ErrorMessage = "Product ID is required.")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Product Status ID is required.")]
    public int StatusId { get; set; } // Foreign Key for ProductStatus
    public Status? Status { get; set; } // Navigation Property to ProductStatus

    [Required(ErrorMessage = "Cost price is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Cost price must be a positive value.")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal CostPrice { get; set; }


    [Required(ErrorMessage = "Retail price is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Retail price must be a positive value.")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal RetailPrice { get; set; }


    [Required(ErrorMessage = "Stock quantity is required.")]
    [Range(0, 9000000, ErrorMessage = "Stock quantity must be between 0 and 1,000,000,000.")]
    [Column(TypeName = "decimal(7,1)")]
    public Decimal StockQuantity { get; set; }

    // Reorder level for the product (stock threshold for reordering)
    [Required]  // Ensures ReorderLevel is provided
    public int ReorderLevel { get; set; }

    // Minimum stock level for the product
    [Required]  // Ensures MinStockLevel is provided
    public int MinStockLevel { get; set; }

    public bool IsWeekly { get; set; } = false;// Indicates whether the product is a weekly inventory item

    public Branch? Branch { get; set; }
    public Product? Product { get; set; }

    public ICollection<BranchProductAuditTrail>? BranchProductAuditTrails { get; set; }
    public ICollection<WeeklyInventory>? WeeklyInventories { get; set; }
    public ICollection<Inventory>? Inventories { get; set; }
}
