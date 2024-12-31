namespace Pacifica.API.Dtos.Product
{
    public class CreateProductDto
    {
        public string? ProductName { get; set; }
        public string? SKU { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public List<int>? BranchIds { get; set; }
        public string? Remarks { get; set; }
        public string? CreatedBy { get; set; }
    
    }
}