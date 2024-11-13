namespace Pacifica.API.Dtos.BranchProduct
{
    public class AddBranchProductDto
    {
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public int StockQuantity { get; set; }
        public string SKU { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}