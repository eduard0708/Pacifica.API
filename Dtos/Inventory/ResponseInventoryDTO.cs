
namespace Pacifica.API.Dtos.Inventory
{
    public class ResponseInventoryDTO : BaseInventoryDTO
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int SystemQuantity { get; set; }
        public decimal Discrepancy { get; set; }
        public decimal SumDiscrepancyValue { get; set; }
        public string? Month { get; set; }  // Assuming Month is an Enum or Class
        public int Week { get; set; }
    }
}