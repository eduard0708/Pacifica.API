namespace Pacifica.API.Models.Inventory
{
    public class YearEndInventory : InventorySnapshot
    {
        public string? YearEnd  { get; set; }  // Track the month (e.g., "January", "February", etc.)

    }
}