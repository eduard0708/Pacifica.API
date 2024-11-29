namespace Pacifica.API.Dtos.BranchProduct
{
    public class GetBranchProductFilterSupplierCategorySKUDto
    {
        public BranchProduct_BranchDto Branch { get; set; } = new BranchProduct_BranchDto();
        public BranchProduct_ProductDto Product { get; set; } = new BranchProduct_ProductDto(); // Nested product DTO
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public int ProductStatusId { get; set; }
        public decimal StockQuantity { get; set; }
        public bool IsActive { get; set; }
    }
}

