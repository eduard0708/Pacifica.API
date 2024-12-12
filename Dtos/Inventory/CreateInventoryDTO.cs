
using static Pacifica.API.Helper.GlobalEnums;

namespace Pacifica.API.Dtos.Inventory
{
    public class CreateInventoryDTO : BaseInventoryDTO
    {
        public InventoryType Type { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}