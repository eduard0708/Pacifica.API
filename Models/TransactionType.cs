using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class TransactionType
    {
        public int Id { get; set; }  // Primary Key
        public string TransactionTypeName { get; set; } = string.Empty;  // Name of the transaction (e.g., Received, Sold, Transferred)
        public string Description { get; set; } = string.Empty;  // Description of the transaction type

        // Navigation property to related stock transactions
        public ICollection<StockTransactionInOut>? StockTransactionInOuts { get; set; }  

        // Audit fields
        [Required]  
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Date when the transaction type was created

        public DateTime? UpdatedAt { get; set; }  // Date when the transaction type was last updated

        public DateTime? DeletedAt { get; set; }  // Soft delete timestamp (null means not deleted)

        [StringLength(100)]  
        public string? CreatedBy { get; set; }  // User who created the transaction type

        [Required]  
        public bool IsActive { get; set; } = true;  // Indicates if the transaction type is active

        [StringLength(100)]  
        public string? UpdatedBy { get; set; }  // User who last updated the transaction type
    }
}
