namespace Pacifica.API.Models.Inventory
{
    public class MidyearInventory : InventorySnapshot
    {
        public string? Midyear { get; set; }  // Track the quarter (e.g., "Q1", "Q2")

    }
}