namespace Pacifica.API.Dtos.Inventory
{
    public class Wi_ResponseSearchBranchProduct
    {
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? SKU { get; set; }
        public decimal StockQuantity { get; set; } 
    }
}