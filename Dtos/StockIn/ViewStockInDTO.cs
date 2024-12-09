
namespace Pacifica.API.Dtos.StockIn
{
    public class ViewStockInDTO
    {
        public int Id { get; set; }
        public string? ReferenceNumber { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public DateTime? DateReported { get; set; }
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public int Quantity { get; set; }
        public int StockInReferenceId { get; set; }
        public string? StockInReferenceName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}