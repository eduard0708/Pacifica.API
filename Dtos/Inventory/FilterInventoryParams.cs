namespace Pacifica.API.Dtos.Inventory
{
    public class FilterInventoryParams
    {
        public int BranchId { get; set; }
        public int? ProductId { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }
    }
}