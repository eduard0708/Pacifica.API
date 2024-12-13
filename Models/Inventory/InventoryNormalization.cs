

using Pacifica.API.Models.GlobalAuditTrails;

namespace Pacifica.API.Models.Inventory
{
    public class InventoryNormalization : AuditDetails
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public DateTime NormalizationDate { get; set; }
        public decimal ActualQuantity { get; set; }
        public decimal SystemQuantity { get; set; }
        public decimal AdjustedQuantity { get; set; }
        public decimal DiscrepancyValue { get; set; }
        public decimal CostPrice { get; set; }

        // Navigation property to InventorySnapshot
        public Inventory? Inventory { get; set; }
    }
}