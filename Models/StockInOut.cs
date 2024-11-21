using System.ComponentModel.DataAnnotations;
using Pacifica.API.Models.GlobalAuditTrails;

namespace Pacifica.API.Models
{
    public class StockInOut : AuditDetails
    {
        public int Id { get; set; }  // Unique identifier for the transaction

        [Required]
        public DateTime TransactionDate { get; set; }  // Date of transaction

        [Required]
        public int StockQuantity { get; set; }  // Quantity of products involved

        [Required]
        public int TransactionNumber { get; set; }  // Unique transaction reference

        public DateTime? DateReported { get; set; }  // Date when the transaction was reported (optional)


        //Navigation 
        [Required]
        public int ProductId { get; set; }  // Product involved in the transaction
        public Product? Product { get; set; }  // Navigation property for Product

        [Required]
        public int StockTransactionTypeId { get; set; }  // Type of the transaction (e.g., In, Out)
        public StockTransactionType? StockTransactionType { get; set; }

        [Required]
        public int ReferenceStockInId { get; set; }  // Reference ID (e.g., Sales Order)
        public ReferenceStockIn? ReferenceStockIn { get; set; }  // Navigation property for TransactionReference

        [Required]
        public int ReferenceStockOutId { get; set; }  // Reference ID (e.g., Sales Order)
        public ReferenceStockOut? ReferenceStockOut { get; set; }  // Navigation property for TransactionReference

        [Required]
        public int TransactionTypeId { get; set; }  // Reference ID (e.g., Sales Order)
        public TransactionType? TransactionType { get; set; }  // Navigation property for TransactionReference

        [Required]
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public IEnumerable<StockInOutAuditTrail>? StockInOutAuditTrails { get; set; }

    }
}
