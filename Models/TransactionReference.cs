using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models
{
    public class TransactionReference
    {
        public int Id { get; set; }  // Primary key
        public string TransactionReferenceName { get; set; } = string.Empty;  // Name of the transaction (e.g., Received, Sold, Transferred)
        public string Description { get; set; } = string.Empty;  // Description of the transaction type

        // Navigation property
        public ICollection<StockInOut>? StockInOuts { get; set; }  // Related stock transactions

        // Audit fields
        [Required]  
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Date the transaction reference was created
        public DateTime? UpdatedAt { get; set; }  // Date the transaction reference was last updated

        public DateTime? DeletedAt { get; set; }  // Soft delete timestamp (null means not deleted)

        [StringLength(100)]  
        public string? CreatedBy { get; set; }  // User who created the transaction reference

        [Required]  
        public bool IsActive { get; set; } = true;  // Indicates if the transaction reference is active

        [StringLength(100)]  
        public string? UpdatedBy { get; set; }  // User who last updated the transaction reference
    }
}
