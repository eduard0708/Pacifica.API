
namespace Pacifica.API.Dtos.Inventory
{
    public class UpdateInventoryDTO : BaseInventoryDTO
    {
        public int SystemQuantity { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdateddAt { get; set; }
    }
}