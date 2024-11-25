
namespace Pacifica.API.Dtos.StockIn
{
    public class StockInUpdateDTO
    {
        public string? ReferenceNumber { get; set; }
        public int ProductId { get; set; }
        public int BranchId { get; set; }
        public DateTime DateReported { get; set; }
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public int StockInReferenceId { get; set; }
        public string? CreatedBy { get; set; }

    }
}