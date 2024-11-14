namespace Pacifica.API.Dtos.StockTransactionInOut
{
    public class GetByReferenceNumberStockTransactionInOutDto
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public int ReferenceNumber { get; set; }
        public int TransactionNumber { get; set; }
        public int TransactionTypeId { get; set; }
        public string? TransactionTypeName { get; set; }

        public int StockQuantity { get; set; }
        public DateTime TransactionDate { get; set; }

        // Additional properties
        public string? BranchName { get; set; }   // For the branch name
        public string? ProductName { get; set; }   // For the product name
        public string? ProductCategory { get; set; } // For the product category
        public string? TransactionReferenceName { get; set; }  // For the transaction reference name
    }
}
