namespace Pacifica.API.Dtos.StockInOut
{
    public class GetByReferenceNumberStockInOutDto
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string? BranchName { get; set; }   // For the branch name
        public int ProductId { get; set; }
        public string? ProductName { get; set; }   // For the product name
        public int ProductCategoryId { get; set; } // For the product category
        public string? ProductCategory { get; set; } // For the product category
        public int ReferenceNumberId { get; set; }
        public string? TransactionReferenceName { get; set; }  // For the transaction reference name
        public int TransactionNumber { get; set; }
        public int TransactionTypeId { get; set; }
        public string? TransactionTypeName { get; set; }
        public int StockQuantity { get; set; }
        public DateTime? DateReported { get; set; }
        public string? Remarks { get; set; }

        public DateTime TransactionDate { get; set; }

    }
}
