using System.ComponentModel.DataAnnotations.Schema;
namespace Pacifica.API.Models.Inventory
{
    public class InventoryNormalization : AuditDetails
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public DateTime NormalizationDate { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal ActualQuantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal SystemQuantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal AdjustedQuantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DiscrepancyValue { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal CostPrice { get; set; }

        // Navigation property to InventorySnapshot
        public Inventory? Inventory { get; set; }
    }
}
