
namespace Pacifica.API.Dtos.StockOut
{
    public class StockOutDTO
    {
        public int Id { get; set; }
        public string? ReferenceNumber { get; set; }
        public int ProductId { get; set; }
        public int BranchId { get; set; }
        public DateTime? DateReported { get; set; }
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public int StockInReferenceId { get; set; }
    }
}