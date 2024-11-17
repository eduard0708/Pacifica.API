namespace Pacifica.API.Dtos.BranchProduct
{
    public class BranchProductDto
    {
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public int ProductStatusId { get; set; }
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public int StockQuantity { get; set; }
        public string? Remarks { get; set; }
        public bool IsActive { get; set; } = true;


    }
}
