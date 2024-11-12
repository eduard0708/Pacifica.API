using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class TransactionType
    {
        public int Id { get; set; }  // Primary Key
        public string TransactionTypeName { get; set; } = string.Empty;  // Name of the transaction (e.g., Received, Sold, Transferred)
        public string Description { get; set; } = string.Empty;     // Description of the transaction type


         // Navigation property
        public ICollection<StockTransactionInOut>? StockTransactionInOuts { get; set; }
        
         // Audit fields
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Soft Delete: Marks when the employee record was deleted (null means not deleted)
        public DateTime? DeletedAt { get; set; }

        // Tracks who created the employee record
        [StringLength(100)] // Maximum length for CreatedBy field
        public string? CreatedBy { get; set; }

        [Required] // Make IsActive a required field
        public bool IsActive { get; set; } = true;

        // Optional: This can help with tracking when the employee was last updated
        [StringLength(100)] // Maximum length for UpdatedBy field
        public string? UpdatedBy { get; set; }

    }
}
