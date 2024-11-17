using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BranchProduct
{

    [Required(ErrorMessage = "Branch ID is required.")]
    public int BranchId { get; set; }
    public Branch? Branch { get; set; }

    [Required(ErrorMessage = "Product ID is required.")]
    public int ProductId { get; set; }
    public Product? Product { get; set; }

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

    [Required(ErrorMessage = "Creation date is required.")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    [StringLength(100, ErrorMessage = "Creator's name cannot exceed 100 characters.")]
    public string? CreatedBy { get; set; }

    [Required(ErrorMessage = "Product active status is required.")]
    public bool IsActive { get; set; } = true;

    [StringLength(100, ErrorMessage = "Updator's name cannot exceed 100 characters.")]
    public string? UpdatedBy { get; set; }
}
