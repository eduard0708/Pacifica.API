
namespace Pacifica.API.Dtos.StockOut
{
    public class StockOutCreateDTO
    {
        public string? ReferenceNumber { get; set; }
        public int ProductId { get; set; }
        public int BranchId { get; set; }
        public DateTime DateReported { get; set; }
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public int StockOutReferenceId { get; set; }
        public string? CreatedBy { get; set; }

    }
}