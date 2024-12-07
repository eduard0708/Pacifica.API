
using System.ComponentModel.DataAnnotations.Schema;

namespace Pacifica.API.Models.Inventory
{
    public class WeeklyInventory : InventorySnapshot
    {
        public int Week { get; set; } // Week number within the month
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal SumDiscrepancyValue { get; set; }

    }
}