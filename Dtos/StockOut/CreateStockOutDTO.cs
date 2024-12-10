
namespace Pacifica.API.Dtos.StockOut
{
    public class CreateStockOutDTO
    {
        public string? ReferenceNumber { get; set; }
        public int ProductId { get; set; }
        public int BranchId { get; set; }
        public int PaymentMethodId { get; set; }
        public int StockOutReferenceId { get; set; }
        public DateTime DateSold { get; set; }
        public int Quantity { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal SoldPrice { get; set; }
        public string? CreatedBy { get; set; }

    }
}