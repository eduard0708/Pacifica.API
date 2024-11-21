
namespace Pacifica.API.Models
{
    public class ReferenceStockOut : AuditDetails
    {
        public int Id { get; set; }  // Primary key
        public string ReferenceStockOutName { get; set; } = string.Empty;  // Name of the transaction (e.g., Received, Sold, Transferred)
        public string Description { get; set; } = string.Empty;  // Description of the transaction type

        // Navigation property
        public ICollection<StockInOut>? StockInOuts { get; set; }  // Related stock transactions
    }
}