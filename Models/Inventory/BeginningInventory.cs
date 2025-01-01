using System.ComponentModel.DataAnnotations.Schema;

namespace Pacifica.API.Models.Inventory
{
    public class BeginningInventory : AuditDetails
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal VeterinaryValue { get; set; }
        public string VeterinaryName { get; set; } = "Veterinary";
        public string VeterinaryIcon { get; set; } = "pi-spin pi-box";
        public string VeterinaryColor { get; set; } = "rgb(239, 84, 81)";


        [Column(TypeName = "decimal(18,2)")]
        public decimal SpecialtyFeedsValue { get; set; }
        public string SpecialtyFeedsName { get; set; } = "Specialty Feeds";
        public string SpecialtyFeedsIcon { get; set; } = "pi-spin pi-file";
        public string SpecialtyFeedsColor { get; set; } = "rgb(21, 225, 55)";

        [Column(TypeName = "decimal(18,2)")]
        public decimal AgriculturalFeedsValue { get; set; }
        public string AgriculturalFeedsName { get; set; } = "Agricultural Feeds";
        public string AgriculturalFeedsIcon { get; set; } = "pi-spin pi-file-check";
        public string AgriculturalFeedsColor { get; set; } = "rgb(40, 69, 234)";


        [Column(TypeName = "decimal(18,2)")]
        public decimal DayOldChicksValue { get; set; }
        public string DayOldChicksName { get; set; } = "Day Old Chick's";
        public string DayOldChicksIcon { get; set; } = "pi-spin pi-file-plus";
        public string DayOldChicksColor { get; set; } = "rgb(191, 15, 185)";

        [Column(TypeName = "decimal(18,2)")]
        public decimal ChemicalsOthersValue { get; set; }
        public string ChemicalsOthersName { get; set; } = "Chemicals and Others";
        public string ChemicalsOthersIcon { get; set; } = "pi-spin pi-flask";
        public string ChemicalsOthersColor { get; set; } = "rgb(58, 140, 241)";


        [Column(TypeName = "decimal(18,2)")]
        public decimal FertilizersSeedsValue { get; set; }
        public string FertilizersSeedsName { get; set; } = "Fertiliser and Seeds";
        public string FertilizersSeedsIcon { get; set; } = "pi-spin pi-seedling";
        public string FertilizersSeedsColor { get; set; } = "rgb(193, 75, 25)";

        public DateTime BeginningInventoryDate { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }

    }
}