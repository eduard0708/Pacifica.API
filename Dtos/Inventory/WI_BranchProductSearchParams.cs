namespace Pacifica.API.Dtos.Inventory
{
    public class WI_BranchProductSearchParams
    {
        public int BranchId { get; set; }
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public string? SKU { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}