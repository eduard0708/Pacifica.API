namespace Pacifica.API.Dtos.BranchProduct
{
    public class BranchProductForStockInDTO
    {
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
    }
}