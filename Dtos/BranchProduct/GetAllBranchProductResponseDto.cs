
namespace Pacifica.API.Dtos.BranchProduct
{
    public class GetAllBranchProductResponseDto
    {
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int CategoryId { get; set; }
        public string? Category { get; set; }
        public int SupplierId { get; set; }
        public string? Supplier { get; set; }
        public int StatusId { get; set; }
        public string? StatusName { get; set; }

        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal StockQuantity { get; set; }
        public int MinStockLevel { get; set; }
        public int ReorderLevel { get; set; }
        public string? SKU { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; }
    }
}