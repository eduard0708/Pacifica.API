

using Pacifica.API.Models.GlobalAuditTrails;

namespace Pacifica.API.Models.Inventory
{
    public class InventoryNormalization : AuditDetails
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public DateTime NormalizationDate { get; set; }
        public decimal AdjustedQuantity { get; set; }
        public decimal DescripancyValue { get; set; }

        // Navigation property to InventorySnapshot
        public Inventory? Inventory { get; set; }
    }
}