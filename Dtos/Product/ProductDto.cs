namespace Pacifica.API.Dtos.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? SKU { get; set; }
        public int CategoryId { get; set; }
        public string? Category { get; set; }
        public int SupplierId { get; set; }
        public string? Supplier { get; set; }
        public string? Remarks { get; set; }
        
    }
}