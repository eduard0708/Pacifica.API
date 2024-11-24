
namespace Pacifica.API.Models.TransactionModels
{
    public class Transaction  : AuditDetails
    {
        public int Id { get; set; }  // Primary key
        public int ProductId { get; set; }  // Foreign key to Product
        public int BranchId { get; set; }  // Foreign key to Branch
        public int? StockInId { get; set; }  // Nullable foreign key to StockIn (if applicable)
        public int? StockOutId { get; set; }  // Nullable foreign key to StockOut (if applicable)
        public decimal? Quantity { get; set; }  // Nullable decimal for quantity (can be null)
        
        public TransactionType TransactionType { get; set; }  // Enum to define the transaction type

        // Navigation Properties
        public StockIn? StockIn { get; set; }
        public StockOut? StockOut { get; set; }
        public Product? Product { get; set; }
        public Branch? Branch { get; set; }
    }
}