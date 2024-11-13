using System.ComponentModel.DataAnnotations;

namespace Pacifica.API.Dtos.StockTransactionInOut
{
    public class CreateStockTransactionInOutDto
    {
        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [Required]
        public int TransactionNumber { get; set; }

        public DateTime? DateReported { get; set; }

        public string Remarks { get; set; } = string.Empty;

        [Required]
        public int ProductId { get; set; }

        [Required]
        public StockTransactionType TransactionType { get; set; }

        [Required]
        public int TransactionReferenceId { get; set; }

        [Required]
        public int BranchId { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

         public string? CreatedBy { get; set; }
    
    }
}