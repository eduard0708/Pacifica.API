
namespace Pacifica.API.Dtos.Inventory
{
    public class UpdateInventoryDTO : BaseInventoryDTO
    {
        
        public string? UpdatedBy { get; set; }
        public DateTime UpdateddAt { get; set; }
    }
}