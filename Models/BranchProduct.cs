using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pacifica.API.Models.GlobalAuditTrails;

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
    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
    public int StockQuantity { get; set; }

    // Reorder level for the product (stock threshold for reordering)
    [Required]  // Ensures ReorderLevel is provided
    public int ReorderLevel { get; set; }

    // Minimum stock level for the product
    [Required]  // Ensures MinStockLevel is provided
    public int MinStockLevel { get; set; }

    public Branch? Branch { get; set; }
    public Product? Product { get; set; }

    public ICollection<BranchProductAuditTrail>? BranchProductAuditTrails { get; set; }
}
