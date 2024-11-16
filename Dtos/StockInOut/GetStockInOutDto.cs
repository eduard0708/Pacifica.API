namespace Pacifica.API.Dtos.StockInOut
{
    public class GetStockInOutDto
    {
        public int TransactionId { get; set; }
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public int TransactionNumber { get; set; }
        public int TransactionTypeId { get; set; }
        public int StockQuantity { get; set; }
        public string? Remarks { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}