using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models.TransactionModels
{
    public class StockIn : AuditDetails
    {
        [Key]
        public int Id { get; set; }

        public TransactionType TransactionType { get; set; }  // Enum to define the transaction type

        [Required]
        [StringLength(50)]
        public string? ReferenceNumber { get; set; }  // Unique reference number for StockIn

        public DateTime DateReported { get; set; }

        public int Quantity { get; set; }  // Nullable decimal for quantity (can be null)

        [Range(2, 18)]
        public decimal CostPrice { get; set; }  // Nullable decimal for cost price (can be null)

        [Range(2, 18)]
        public decimal RetailPrice { get; set; }  // Nullable decimal for retail price (can be null)

        [Required]
        public int ProductId { get; set; }  // Foreign key to Product

        [Required]
        public int BranchId { get; set; }  // Foreign key to Branch


        [Required]
        public int StockInReferenceId { get; set; }  // Foreign key to Branch

        // Navigation Properties
        public Product? Product { get; set; }
        public Branch? Branch { get; set; }
        public StockInReference? StockInReference { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }  // One StockIn can have many transactions
    }
}