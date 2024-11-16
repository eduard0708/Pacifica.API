using Pacifica.API.Dtos.StockTransactionInOut;

namespace Pacifica.API.Dtos.StockInOut

{
    public class TransactionGetbyReferenceNumber
    {
        public int Id { get; set; }
        public Transaction_BranchDto Branch { get; set; } = new Transaction_BranchDto();
        public Transaction_ProductDto Product { get; set; } = new Transaction_ProductDto();
        public Transaction_TypeDto TransactionType { get; set; } = new Transaction_TypeDto();
        public Transaction_ReferenceDto TransactionReference { get; set; } = new Transaction_ReferenceDto();
        public int TransactionNumber { get; set; }
        public int StockQuantity { get; set; }
        public DateTime? ReportedDate { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}