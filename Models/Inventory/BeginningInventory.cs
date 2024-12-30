using System.ComponentModel.DataAnnotations.Schema;

namespace Pacifica.API.Models.Inventory
{
    public class BeginningInventory : AuditDetails
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal VeterinaryValue { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal SpecialtyFeedsValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AgriculturalFeedsValue { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DayOldChicksValue { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ChemicalsOthersValue { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal FertilizersSeedsValue { get; set; }
        public DateTime BeginningInventoryDate { get; set; }

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

    }
}