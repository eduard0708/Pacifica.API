namespace Pacifica.API.Dtos.Inventory
{
    public class ResponseViewInventoryDTO
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public string? SKU { get; set; }
        public DateTime InventoryDate { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SystemQuantity { get; set; }
        public decimal Discrepancy { get; set; }
        public int WeekNumber { get; set; }
        public decimal SumDiscrepancyValue { get; set; }
        public string? Remarks { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}