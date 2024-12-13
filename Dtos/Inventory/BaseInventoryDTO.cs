using static Pacifica.API.Helper.GlobalEnums;

namespace Pacifica.API.Dtos.Inventory
{
    public class BaseInventoryDTO
    {

        public int BranchId { get; set; }
        public int ProductId { get; set; }
        public int Week { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int SystemQuantity { get; set; }
        public int ActualQuantity { get; set; }
        public decimal Discrepancy { get; set; }
        public decimal DiscrepancyValue { get; set; }
        public DateTime InventoryDate { get; set; }
        public InventoryType Type { get; set; }
        public string? Remarks { get; set; }

    }
}