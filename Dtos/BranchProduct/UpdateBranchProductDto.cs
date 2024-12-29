
namespace Pacifica.API.Dtos.BranchProduct
{
    public class UpdateBranchProductDto
    {
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int CategoryId { get; set; }
        public string? Category { get; set; }
        public int SupplierId { get; set; }
        public string? Supplier { get; set; }
        public string? Sku { get; set; }
        public int StatusId { get; set; }
        public string? Status { get; set; }
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public int StockQuantity { get; set; }
        public int MinStockLevel { get; set; }
        public int ReorderLevel { get; set; }
        public string? Remarks { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}