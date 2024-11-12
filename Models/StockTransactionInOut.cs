namespace Pacifica.API.Models
{
    public class StockTransactionInOut
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public int Quantity { get; set; }
        public int TransactionNumber { get; set; }   // Transaction reference number
        public DateTime? DateReported { get; set; }
        public int CreatedBy { get; set; }  // User who created the transaction
        

        // Navigation property
        public int ProductId { get; set; }  
        public Product? Product { get; set; }

        public int TransactionTypeId { get; set; }
        public TransactionType? TransactionType { get; set; }

        public int TransactionReferenceId { get; set; }
        public TransactionReference? TransactionReference { get; set; }

    }

}