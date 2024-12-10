
namespace Pacifica.API.Dtos.BranchProduct
{
    public class GetFilter_Products
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? SKU { get; set; }
        public Product_CategoryDto Category { get; set; } = new Product_CategoryDto();
        
    }
}