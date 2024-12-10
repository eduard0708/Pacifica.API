
namespace Pacifica.API.Dtos.StockOut
{
    public class ViewStockOutDTO
    {
        public int Id { get; set; }
        public string? ReferenceNumber { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int StockOutReferenceId { get; set; }
        public string? StockOutReferenceName { get; set; }
        public int PaymentMethodId { get; set; }
        public string? PaymentMethodName { get; set; }
        public DateTime DateSold { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal SoldPrice { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}