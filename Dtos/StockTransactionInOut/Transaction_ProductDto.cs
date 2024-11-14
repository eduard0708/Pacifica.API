using Pacifica.API.Dtos.BranchProduct;

namespace Pacifica.API.Dtos.StockTransactionInOut
{
    public class Transaction_ProductDto
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public Product_CategoryDto Category { get; set; } = new Product_CategoryDto();
        public Product_StatusDto Status { get; set; } = new Product_StatusDto();
        public string? SKU { get; set; }
    }
}