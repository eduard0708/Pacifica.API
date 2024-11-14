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
    public int ProductStatusId { get; set; } // Foreign Key for ProductStatus
    public ProductStatus? ProductStatus { get; set; } // Navigation Property to ProductStatus

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



// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;

// namespace Pacifica.API.Models
// {
//     public class BranchProduct
//     {
//         // Foreign Key to the Branch entity (Many-to-One relationship)
//         [Required(ErrorMessage = "Branch ID is required.")]
//         public int BranchId { get; set; }
//         public Branch? Branch { get; set; }

//         [Required(ErrorMessage = "Product ID is required.")]
//         public int ProductId { get; set; }
//         public Product? Product { get; set; }

//         [Required(ErrorMessage = "Product ID is required.")]
//         public int ProductStatusId { get; set; }
//         public ProductStatus? ProductStatus { get; set; }

//         // Cost Price of the product in the branch (Required field)
//         [Required(ErrorMessage = "Cost price is required.")]
//         [Range(0, double.MaxValue, ErrorMessage = "Cost price must be a positive value.")]
//         [Column(TypeName = "decimal(18,2)")]  // Specifies decimal precision and scale
//         public decimal CostPrice { get; set; }

//         // Retail Price of the product in the branch (Required field)
//         [Required(ErrorMessage = "Retail price is required.")]
//         [Column(TypeName = "decimal(18,2)")]  // Specifies decimal precision and scale
//         [Range(0, double.MaxValue, ErrorMessage = "Retail price must be a positive value.")]
//         public decimal RetailPrice { get; set; }

//         // Stock Quantity of the product in the branch (Required field)
//         [Required(ErrorMessage = "Stock quantity is required.")]
//         [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
//         public int StockQuantity { get; set; }


//         // Audit Fields

//         // Date when the record was created
//         [Required(ErrorMessage = "Creation date is required.")]
//         public DateTime CreatedAt { get; set; } = DateTime.Now;

//         // Date when the record was last updated (optional)
//         public DateTime? UpdatedAt { get; set; }

//         // Date when the record was deleted (optional, for soft delete purposes)
//         public DateTime? DeletedAt { get; set; }

//         // The user who created the record (optional, max length 100)
//         [StringLength(100, ErrorMessage = "Creator's name cannot exceed 100 characters.")]
//         public string? CreatedBy { get; set; }

//         // Is the product currently active in the branch (Required field)
//         [Required(ErrorMessage = "Product active status is required.")]
//         public bool IsActive { get; set; } = true;

//         // The user who last updated the record (optional, max length 100)
//         [StringLength(100, ErrorMessage = "Updator's name cannot exceed 100 characters.")]
//         public string? UpdatedBy { get; set; }
//     }
// }