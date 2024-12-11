namespace Pacifica.API.Dtos.Inventory
{
    public class BaseInventoryDTO
    {
        
        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public DateTime InventoryDate { get; set; }
        public int ActualQuantity { get; set; }
        public string? Remarks { get; set; }

    }
}