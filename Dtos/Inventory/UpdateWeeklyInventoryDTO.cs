
namespace Pacifica.API.Dtos.Inventory
{
    public class UpdateWeeklyInventoryDTO : BaseInventoryDTO
    {
        public int SystemQuantity { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdateddAt { get; set; }
    }
}