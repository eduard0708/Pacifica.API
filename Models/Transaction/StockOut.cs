using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Models.Transaction
{
    public class StockOut : AuditDetails
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string? ReferenceNumber { get; set; }

        public int Quantity { get; set; }

        [StringLength(100)]
        public string? TransactionName { get; set; }

        public string? TransactionType { get; set; }  // Enum to define the transaction type

        [Required]
        public int? ProductId { get; set; }

        [Required]
        public int PaymentTypeId { get; set; }

        [Required]
        public int StockOutReferenceId { get; set; }

        [Required]
        public int? BranchId { get; set; }

        [Range(18, 2)]
        public decimal RetailPrice { get; set; }

        [Required]

        public DateTime? DateReported { get; set; }

        // Navigation properties
        public Product? Product { get; set; }
        public Branch? Branch { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public StockOutReference? StockOutReference { get; set; }
    }
}
