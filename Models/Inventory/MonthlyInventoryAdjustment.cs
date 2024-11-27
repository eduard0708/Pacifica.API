
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pacifica.API.Models.Inventory

{
    public class MonthlyInventoryAdjustment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Branch ID is required.")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Stock quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Inventory adjustment date is required.")]
        public DateTime AdjustmentDate { get; set; }

        // Difference between the expected stock (based on Beginning Inventory) and actual stock counted
        public int Difference { get; set; }

        [Required(ErrorMessage = "Adjusted by field is required.")]
        public string? AdjustedBy { get; set; }  // You can store the user's name or ID who made the adjustment

        // Reason for the adjustment (e.g., stock count, damage, theft, etc.)
        [StringLength(500)]
        public string? AdjustmentReason { get; set; }

        [ForeignKey("BranchId")]
        public Branch? Branch { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        // Navigation property for audit trail, if needed
        public ICollection<BranchProductInventoryAuditTrail>? BranchProductAuditTrails { get; set; }
    }

}