namespace Pacifica.API.Dtos.Product
{
    public class CreateProductDto
    {
        public string? ProductName { get; set; }
        // public decimal CostPrice { get; set; }
        // public decimal RetailPrice { get; set; }
        // public int StockQuantity { get; set; }
        public string? SKU { get; set; }
        public string? ProductStatus { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }


    }
}