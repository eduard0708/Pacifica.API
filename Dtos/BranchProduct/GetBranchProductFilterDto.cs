namespace Pacifica.API.Dtos.BranchProduct
{
    public class GetBranchProductFilterDto
    {
        public int BranchId { get; set; }
        public string? BranchName { get; set; }
        public BranchProduct_ProductDto Product { get; set; } = new BranchProduct_ProductDto(); // Nested product DTO
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public int ProductStatusId { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
    }
}

