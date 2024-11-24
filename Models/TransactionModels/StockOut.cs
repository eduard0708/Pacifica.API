using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pacifica.API.Models.TransactionModels
{
    public class StockOut : AuditDetails
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string? ReferenceNumber { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive value.")]
        public int Quantity { get; set; }

        [StringLength(100)]
        public string? TransactionName { get; set; }

        public TransactionType TransactionType { get; set; }  // Enum to define the transaction type

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int PaymentTypeId { get; set; }

        [Range(2, 18)]
        public decimal RetailPrice { get; set; }  // Nullable decimal for retail price (can be nu

        [Required]
        public int BranchId { get; set; }

        [Required]
        public int StockOutReferenceId { get; set; }

        public DateTime? DateReported { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        [ForeignKey("BranchId")]
        public Branch? Branch { get; set; }

        [ForeignKey("PaymentTypeId")]
        public PaymentType? PaymentType { get; set; }

        [ForeignKey("StockOutReferenceId")]
        public StockOutReference? StockOutReference { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }
    }
}
