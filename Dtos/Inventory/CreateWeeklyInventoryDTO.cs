
namespace Pacifica.API.Dtos.Inventory
{
    public class CreateWeeklyInventoryDTO : BaseInventoryDTO
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}