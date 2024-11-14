
namespace Pacifica.API.Dtos.BranchProduct
{
    public class GetFilter_Products
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public Product_CategoryDto Category { get; set; } = new Product_CategoryDto();
        public Product_StatusDto Status { get; set; } = new Product_StatusDto();
        public string? SKU { get; set; }
    }
}