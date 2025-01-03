namespace Pacifica.API.Models
{
    public class TransactionType : AuditDetails
    {
        public int Id { get; set; }  // Primary key
        public string TransactionTypeName { get; set; } = string.Empty;  // Name of the transaction (e.g., Received, Sold, Transferred)
        public string Description { get; set; } = string.Empty;  // Description of the transaction type
       
    }
}
