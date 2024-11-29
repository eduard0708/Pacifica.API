namespace Pacifica.API.Models.Inventory
{
    public class MonthlyInventory : InventorySnapshot
    {
        public Months Monthly { get; set; }  // Track the month (e.g., "January", "February", etc.)

    }
}