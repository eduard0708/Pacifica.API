namespace Pacifica.API.Dtos.BranchProduct
{
    public class BranchProduct_ProductDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? Remarks { get; set; }
        public string? SKU { get; set; }
    }
}
