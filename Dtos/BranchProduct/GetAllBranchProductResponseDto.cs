
namespace Pacifica.API.Dtos.BranchProduct
{
    public class GetAllBranchProductResponseDto
    {
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public int ProductId { get; set; }
        public int ProductStatusId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCategory { get; set; }
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public int StockQuantity { get; set; }
        public string? SKU { get; set; }
        public bool IsActive { get; set; }
    }
}