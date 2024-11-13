using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class StockTransactionInOut
    {
        public int Id { get; set; }  // Unique identifier for the transaction

        [Required]
        public DateTime TransactionDate { get; set; }  // Date of transaction

        [Required]
        public int StockQuantity { get; set; }  // Quantity of products involved

        [Required]
        public int TransactionNumber { get; set; }  // Unique transaction reference

        public DateTime? DateReported { get; set; }  // Date when the transaction was reported (optional)

        public string Remarks { get; set; } = string.Empty;  // Additional transaction notes

        //Navigation 
        [Required]
        public int ProductId { get; set; }  // Product involved in the transaction
        public Product? Product { get; set; }  // Navigation property for Product

        [Required]
        public StockTransactionType StockTransactionType { get; set; }  // Type of the transaction (e.g., In, Out)
        
        [Required]
        public int TransactionReferenceId { get; set; }  // Reference ID (e.g., Sales Order)
        public TransactionReference? TransactionReference { get; set; }  // Navigation property for TransactionReference

        [Required]
        public int BranchId { get; set; } 
        public Branch? Branch { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Date when the transaction was created

        public DateTime? UpdatedAt { get; set; }  // Date when the transaction was last updated

        public DateTime? DeletedAt { get; set; }  // Soft delete timestamp (null if active)

        [StringLength(100)]
        public string? CreatedBy { get; set; }  // User who created the transaction

        [Required]
        public bool IsActive { get; set; } = true;  // Indicates if the transaction is active

        [StringLength(100)]
        public string? UpdatedBy { get; set; }  // User who last updated the transaction

        
    }
}
