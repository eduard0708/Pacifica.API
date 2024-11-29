
namespace Pacifica.API.Models.Inventory
{
    public class WeeklyInventory :InventorySnapshot
    {
     public int Week { get; set; } // Week number within the month
     public decimal SumDiscrepancyValue { get; set; }

    }
}