namespace Pacifica.API.Dtos.BranchProduct
{
    public class BranchProduct_ProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public BranchProduct_CategoryDto Category { get; set; } = new BranchProduct_CategoryDto();
        public BranchProduct_StatusDto Status { get; set; } = new BranchProduct_StatusDto();
        public string? SKU { get; set; }
    }
}
