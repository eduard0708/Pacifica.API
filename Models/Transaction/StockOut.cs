using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pacifica.API.Models.GlobalAuditTrails;

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
        public int PaymentMethodId { get; set; }

        [Required]
        public int StockOutReferenceId { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal RetailPrice { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal SoldPrice { get; set; }

        [Required]
        public DateTime DateSold { get; set; }

        // Navigation properties
        public Product Product { get; set; } = new Product();
        public Branch Branch { get; set; } = new Branch();
        public PaymentMethod? PaymentMethod { get; set; }
        public StockOutReference? StockOutReference { get; set; }
        public ICollection<StockOutAuditTrail>? StockOutAuditTrails { get; set; }
    }
}
